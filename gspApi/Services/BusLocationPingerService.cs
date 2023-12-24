namespace gspAPI.Services;

using System.Diagnostics;
using Entities;
using Models;
using Newtonsoft.Json;
using gspAPI.Utils;
using gspApiGetter.BusTableAPI;
using Microsoft.EntityFrameworkCore;

public class BusLocationPingerService : IHostedService
{
    readonly IServiceProvider _services;
    // so it doesn't get garbage collected
    // ReSharper disable once NotAccessedField.Local
    Timer? _timer;
    readonly HttpClient _client = new();
    private DateTime _lastTime = DateTime.Now;
    
    readonly ILogger<BusLocationPingerService> _logger;

    public BusLocationPingerService(IServiceProvider Services)
    {
        _services = Services;
        var scope = _services.CreateScope();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<BusLocationPingerService>>();
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add(
            "X-Api-Authentication","1688dc355af72ef09287" 
        );

    }


    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _services.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IBusTableRepository>();
            var count = repository.getCountBusTables();
            if (count == 0)
            {
                var getter = scope.ServiceProvider.GetRequiredService<IBusTableGetter>();
                await getter.updateAllTables();
            }
        }

        _timer = new Timer(ping,
            null,
            0,
            60000);
        // var tasklist = ping();
        // Task.WaitAll(tasklist);
        // while (true)
        // {
        //
        //     if(DateTime.Now.Subtract(_lastTime).TotalSeconds >= 60)
        //     {
        //         tasklist = ping();
        //         Task.WaitAll(tasklist);
        //         _lastTime = DateTime.Now;
        //     }
        //     Thread.Sleep(5000);
        //     

        // }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

   
    async void handleResponse(HttpResponseMessage response,List<BusTable> busTables,Time time,bool oppositeDirection = false)
    {

        using (var scope = _services.CreateScope())
        {
            var _repository = scope.ServiceProvider.GetRequiredService<IBusTableRepository>();
            var requestUri = response.RequestMessage!.RequestUri!.ToString();
            _logger.LogInformation(
                $"Handling {requestUri} routes:{string.Join(", ", busTables.Select(b => b.BusRoute.NameShort))}");
            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Failed to reach {requestUri}: ${response.StatusCode}");
            List<VehiclesApiResponse.Root> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<VehiclesApiResponse.Root>>(
                    await response.Content.ReadAsStringAsync())!;

                if (json[0].code == 3)
                {
                    _logger.LogInformation($"Got code 3 with {requestUri}");
                    return;
                }

                if (json[0].line_number == null)
                {
                    _logger.LogInformation($"Skipping json response: Failed validation (can happen if API decided to not respond with coordinates)");
                    return;
                }
            }
            catch (Exception)
            {
                _logger.LogInformation(
                    $"Failed to deserialize {requestUri},\ncontents:\n{await response.Content.ReadAsStringAsync()} ");
                return;
            }

            BusTable matched = null!;
            // Start from the back to get the nearest buses
            for (int i = json.Count - 1; i >= 0; i--)

            {
                var entry = json[i];

               
                // Our BusTables lines don't know the difference between Night buses and other weirdly named ones.
                
                if (entry.line_number.EndsWith("a") || entry.line_number.EndsWith("b"))
                    entry.line_number = entry.line_number.Remove(entry.line_number.Length - 1,
                        1);
                if (entry.line_number.EndsWith("N"))
                    entry.line_number = entry.line_number.Remove(entry.line_number.Length - 1,
                        1);
                // Uses RemoveALl from gspApi.Utils.ListExtensions !!!
                // It's always going to match exactly one if it exists.
                if (busTables.RemoveAll(b => b.BusRoute.NameShort == entry.line_number,
                        b => matched = b) == 0)
                {
                    _logger.LogInformation($"Skipping entry in api response {entry.line_number}");
                    continue;
                }

                if (matched == null) throw new UnreachableException();
                _logger.LogInformation("creating ping");
                var ping = new PingCache();
                if(entry.stations_gpsx != null){
                    var lat = entry.stations_gpsx;
                    var lon = entry.stations_gpsy;
                    if (lat == null || lon == null)
                    {
                        ping = new PingCache()
                        {
                        
                            BusTableId = matched.BusTableId,
                            TimeId = time.TimeId,
                            Lat = 999,
                            Lon = 999,
                            Distance = 999,
                            GotFromOppositeDirection = oppositeDirection,
                            StationsBetween = entry.stations_between
                        
                        };
                    }
                    else
                    {
                        if (!entry.validate())
                        {
                            _logger.LogInformation("Failed to validate json entry... skipping.");
                            return;
                        }
                        ping = new PingCache()
                        {
                        
                            BusTableId = matched.BusTableId,
                            TimeId = time.TimeId,
                            Lat = float.Parse(lat),
                            Lon = float.Parse(lon),
                            Distance = (float)entry.DistanceTo(matched.BusStop.Lat,
                                matched.BusStop.Lon),
                            GotFromOppositeDirection = oppositeDirection,
                            StationsBetween = entry.stations_between
                        
                        };
                    }
                }
                else ping = new PingCache() { BusTableId = matched.BusTableId, TimeId = time.TimeId, Lat = 999, Lon = 999, Distance = 999, GotFromOppositeDirection = oppositeDirection, StationsBetween = entry.stations_between };

                _repository.addPingCache(ping);
                _logger.LogInformation($"Added ping on {matched.BusTableId}");
                break;
            }

            // If the bus was not found, check the last bus in the opposite direction ( which the closest bus to the current station )
            if (busTables.Any() && oppositeDirection == false)
            {
                _logger.LogInformation("Checking opposite direction");
                foreach (var busTable in busTables)
                {
                    var oppositeBt = await _repository.getOppositeDirectionBusTable(busTable);
                    if (oppositeBt == null)
                    {
                        _logger.LogError($"Couldn't find opposite direction for BusTable {busTable.BusTableId}");
                        continue;
                    }

                    await makePing(oppositeBt.BusStop.BusStopId.ToString(),
                        new List<BusTable>() { busTable },
                        time,
                        oppositeDirection: true);
                }

                return;
            }
            else if (busTables.Any() && oppositeDirection == true)
            {
                foreach (var busTable in busTables)
                {
                    _logger.LogInformation(
                        $"Failed to find {busTable.BusRoute.NameShort} for BusTableId={busTable.BusTableId}");

                    var ping = new PingCache()
                    {
                        BusTableId = busTable.BusTableId,
                        TimeId = time.TimeId,
                        Lat = 0,
                        Lon = 0,
                        Distance = 999,
                        GotFromOppositeDirection = oppositeDirection,
                        StationsBetween = 999

                    };
                    _repository.addPingCache(ping);
                }
            }

                    await _repository.saveChangesAsync();
        }
    }
    private async void ping(Object? obj )
    {

        using(var scope = _services.CreateScope())
        {
            var _repository = scope.ServiceProvider.GetRequiredService<IBusTableRepository>();
            var date = DateTime.Now;
            int hour = date.Hour;
            int minute = date.Minute;
            string day = date.DayOfWeek.ToString();
            int dayid = 1;
            if (day == "Saturday") dayid = 2;
            if (day == "Sunday") dayid = 3;

            var time = await _repository.getTime(hour,
                minute,
                dayid);
        
            // BusStop: BusTable[]
            var toCheck = await _repository.getBusTablesByTime(time);
            foreach (var bsGroup in toCheck)
            {
                makePing(bsGroup.Key.BusStopId.ToString()!,bsGroup.Value,time);
            }

            
        }
    }
    

    private Task makePing(string busStopIdS,List<BusTable> busTables,Time time,bool oppositeDirection = false)
    {
        
        var uid = "2";
        var toPad = 4 - busStopIdS.Length;
        for (int i = 0; i < toPad; i++)
        {
            uid += "0";
        }

        uid += busStopIdS;
        var url = $"https://online.bgnaplata.rs/publicapi/v1/announcement/announcement.php?station_uid={uid}&action=get_announcement_data";
        _logger.LogInformation("Getting " + url);
        // _logger.LogInformation("Checking " + url);
        // ReSharper disable once UnusedVariable
        return _client.GetAsync(url).ContinueWith(response =>handleResponse(response.Result,busTables,time,oppositeDirection));
    }


}
