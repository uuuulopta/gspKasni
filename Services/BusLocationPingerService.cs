namespace gspAPI.Services;

using System.Diagnostics;
using Entities;
using Models;
using Newtonsoft.Json;
using gspAPI.Utils;

public class BusLocationPingerService : IHostedService
{   
    // so it doesn't get garbage collected
    // ReSharper disable once NotAccessedField.Local
    private Timer? _timer;
    readonly HttpClient _client = new();
    private ILogger<BusLocationPingerService> _logger;
    private IBusTableRepository _repository;
    public BusLocationPingerService(IServiceProvider services)
    {
        Services = services;
        var scope = Services.CreateScope();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<BusLocationPingerService>>();
        _repository = scope.ServiceProvider.GetRequiredService<IBusTableRepository>() ?? throw new NullReferenceException();
        
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add(
            "X-Api-Authentication","1688dc355af72ef09287" 
        );

    }


    public IServiceProvider Services { get; } 
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(ping,
            null,
            0,
            60000);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

   
    async void handleResponse(HttpResponseMessage response,List<BusTable> busTables,Time time)
    {
        var requestUri = response.RequestMessage!.RequestUri!.ToString();
        _logger.LogInformation($"Handling {requestUri} routes:{string.Join(", ",busTables.Select(b => b.BusRoute.NameShort))}");
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
        }
        catch (Exception) {
            _logger.LogInformation($"Failed to deserialize {requestUri},\ncontents:\n{await response.Content.ReadAsStringAsync()} ");
            return;
        }
        
        BusTable matched = null!;
        for(int i = json.Count-1; i >= 0; i--)
        {
            var entry = json[i];
            // Uses RemoveALl from gspApi.Utils.ListExtensions !!!
            // It's always going to match one.
            if (busTables.RemoveAll(b => b.BusRoute.NameShort == entry.line_number, b => matched = b) == 0)
            {
                _logger.LogInformation($"Skipping entry in api response {entry.line_number}");
                continue;
            }

            if (matched == null) throw new UnreachableException();
            _logger.LogInformation("creating ping");
            var ping = new PingCache()
            {
                BusTable = matched,
                Time = time,
                Lat = float.Parse(entry.stations_gpsx),
                Lon = float.Parse(entry.stations_gpsy),
                Distance = (float)entry.DistanceTo(matched.BusStop.Lat,matched.BusStop.Lon),
                StationsBetween = entry.stations_between
                
            };
            
            _logger.LogInformation($"Added ping on {matched.BusTableId}");
            await _repository.addPingCache(ping);
            
            break;
        }
// TODO check in opposite direction!
        await _repository.saveChangesAsync();

    }
    private async void ping(Object? state)
    {
        
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
            var uid = "2";
            var busStopIdS = bsGroup.Key.BusStopId.ToString();
            var toPad = 4 - busStopIdS.Length;
            for (int i = 0; i < toPad; i++)
            {
                uid += "0";
            }

            uid += busStopIdS;
            var url = $"https://online.bgnaplata.rs/publicapi/v1/announcement/announcement.php?station_uid={uid}&action=get_announcement_data";
            _logger.LogInformation("Checking " + url);
            // ReSharper disable once UnusedVariable
            var get = _client.GetAsync(url).ContinueWith(response =>handleResponse(response.Result,bsGroup.Value,time));
        }

    }
     
}
