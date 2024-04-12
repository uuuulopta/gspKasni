namespace gspAPI.Services;

using System.Diagnostics;
using BusTableAPI;
using Entities;
using Models;
using Newtonsoft.Json;
using Utils;

public class BusLocationPingerService : IHostedService
{
    readonly IServiceProvider _services;
    // so they don't get garbage collected
    // ReSharper disable once NotAccessedField.Local
    Timer? _timer;
    bool _pingWorking;
    // ReSharper disable once NotAccessedField.Local
    Timer? _timerUpdate;
    readonly HttpClient _client = new();
    bool _updatingBustables;
    volatile List<Task> _requests = new(); 
    readonly ILogger<BusLocationPingerService> _logger;

    public BusLocationPingerService(IServiceProvider services)
    {
        _services = services;
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
            30000);
        
        
        _timerUpdate = new Timer(updateTables,
            null,
            0,
            // 1 day
            86400000);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    void updateTables(object? obj)
    {
        lock (_requests)
        {
            Task.WaitAll(_requests.ToArray());
            _updatingBustables = true;
            using (var scope = _services.CreateScope())
            {
                var busTableGetter = scope.ServiceProvider.GetRequiredService<IBusTableGetter>();
                busTableGetter.updateAllTables().Wait();
            }
            
            _updatingBustables = false;
        }

    }
   
    async void handleResponse(HttpResponseMessage response,string uid,List<BusTable> busTables,Time time,bool oppositeDirection = false)
    {
        
        using (var scope = _services.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IBusTableRepository>();

            _logger.LogInformation($"Handling {uid} opposite={oppositeDirection} routes:{string.Join(", ",busTables.getRouteNameShort())}");
            
            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Failure: ${response.StatusCode}");
            
            var encrypted = await response.Content.ReadAsStringAsync();
            
            if (String.IsNullOrWhiteSpace(encrypted))
            {
                _logger.LogError($"Response for {uid} is empty.");
                return;
            }
            
            // Decrypt api response
            var content = ApiResponseDecryptors.decrpytBulletinResponse(encrypted);
            VehiclesApiv2Response.Root json;
            try
            {
                json = JsonConvert.DeserializeObject<VehiclesApiv2Response.Root>(content)!;
                if (json.code != 1)
                {
                    _logger.LogInformation($"Response for {uid} didn't respond with code=1, skipping.");
                    return;
                }
                if (json.data.Count == 0)
                {
                    _logger.LogInformation($"Response for {uid} didn't contain any data, skipping.");
                    return;
                }

    
            }
            catch (Exception)
            {
                _logger.LogError(
                    $"Failed to deserialize response for {uid},\ncontents:\n{content} ");
                return;
            }

            BusTable matched = null!;
            // just_cordinates=1 when we get no bus location response
            if (json.data[0].just_coordinates == 0)
            {
                // API response is ordered by distance, so start from the back to get the nearest buses
                for (int i = json.data.Count - 1; i >= 0; i--)
                {
                    var entry = json.data[i];

                    if (entry.line_number == null)
                    {
                        _logger.LogError($"Received non-valid data api response: {content}");
                        return;
                    }
               
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
                        _logger.LogDebug($"Skipping entry in api response {entry.line_number}");
                        continue;
                    }

                    if (matched == null) throw new UnreachableException();
                    // ReSharper disable once InconsistentNaming
                    int stations_between = 0;
                    if (oppositeDirection)
                    {
                        stations_between = entry.stations_between + entry.all_stations.Count;
                    }
                    var ping = new PingCache
                    {
                        BusTableId = matched.BusTableId,
                        TimeId = time.TimeId,
                        Lat = float.Parse(entry.vehicles[0].lat),
                        Lon = float.Parse(entry.vehicles[0].lng),
                        Distance = (float)entry.DistanceTo(matched.BusStop.Lat, matched.BusStop.Lon),
                        GotFromOppositeDirection = oppositeDirection,
                        StationsBetween = stations_between,
                    };

                    repository.addPingCache(ping);
                    _logger.LogInformation($"Added ping on {matched.BusTableId}");
                  
                }
            }

            // If the bus was not found, check the last bus in the opposite direction ( which the closest bus to the current station )
            if (busTables.Any() && oppositeDirection == false)
            {
                
                _logger.LogInformation($"Checking opposite direction for {uid}");
                foreach (var busTable in busTables)
                {
                    var oppositeBt = await repository.getOppositeDirectionBusTable(busTable);
                    if (oppositeBt == null)
                    {
                        _logger.LogWarning($"Couldn't find opposite direction for BusTable {busTable.BusTableId}");
                        continue;
                    }

                    makePing(oppositeBt.BusStop.BusStopId.ToString(),
                        new List<BusTable>() { busTable },
                        time,
                        oppositeDirection: true);
                }

                return;
            }
            else if (busTables.Any() && oppositeDirection)
            {
                foreach (var busTable in busTables)
                {
                    _logger.LogInformation(
                        $"Failed to find {busTable.BusRoute.NameShort} for BusTableId={busTable.BusTableId}, creating badping");

                    var ping = PingCache.createBadPingCache(busTable,time,oppositeDirection); 
                    repository.addPingCache(ping);
                }
            }

            await repository.saveChangesAsync();
        }
    }
    private async void ping(Object? obj )
    {

        
        if (_updatingBustables || _pingWorking) return;
        _pingWorking = true;
        lock (_requests)
        {
            _requests.RemoveAll(x => x.IsCompleted);
            
        }
        using(var scope = _services.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IBusTableRepository>();
            var date = DateTime.Now;
            int hour = date.Hour;
            int minute = date.Minute;
            string day = date.DayOfWeek.ToString();
            int dayid = 1;
            if (day == "Saturday") dayid = 2;
            if (day == "Sunday") dayid = 3;

            var time = await repository.getTime(hour,
                minute,
                dayid);
            if (time == null)
            {
                _pingWorking = false;
                return;
            } 
            // BusStop: BusTable[]
            var toCheck = await repository.getBusTablesByTime(time);
            foreach (var bsGroup in toCheck)
            {
                makePing(bsGroup.Key.BusStopId.ToString(),bsGroup.Value,time);
            }

            
        }
        _pingWorking = false;
    }
    

    private void makePing(string busStopIdS,List<BusTable> busTables,Time time,bool oppositeDirection = false)
    {
        string uid = convert_station_uid(busStopIdS); 
        var request = ApiPayloads.bulletinPayload(uid);
        lock(_requests)
        {
            _requests.Add(
                _client.SendAsync(request)
                    .ContinueWith(response =>handleResponse(response.Result,uid,busTables,time,oppositeDirection))
            );
        }
    }

    /// <summary>
    /// Converts gtfs database uids to GSP api uids
    /// </summary>
    /// <returns></returns>
    private string convert_station_uid(string busStopIdS)
    {
        
        var uid = "2";
        var toPad = 4 - busStopIdS.Length;
        for (int i = 0; i < toPad; i++)
        {
            uid += "0";
        }
        uid += busStopIdS;
        return uid;
    }

}
