namespace gspAPI.Services;
using Entities;
using Models;
using Newtonsoft.Json;

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

    async void handleResponse(HttpResponseMessage response,BusTable bt,Time time)
    {
    
        var requestUri = response.RequestMessage!.RequestUri!.ToString();
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
        foreach (var entry in json)
        {
            if (entry.line_number != bt.BusRoute.NameShort)
            {
                _logger.LogInformation($"Skipping entry in api response {entry.line_number} != {bt.BusRoute.NameShort}");
                continue;
            } 
            _logger.LogInformation("creating ping");
            var ping = new PingCache()
            {
                BusTable = bt,
                Time = time,
                Lat = float.Parse(entry.stations_gpsx),
                Lon = float.Parse(entry.stations_gpsy),
                Distance = (float)entry.DistanceTo(bt.BusStop.Lat,bt.BusStop.Lon),
                StationsBetween = entry.stations_between
                
            };
            
            _logger.LogInformation($"Added ping on {bt.BusTableId}");
            await _repository.addPingCache(ping);
        }

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
        var toCheck = await _repository.getBusTablesByTime(time);
        foreach (var busTable in toCheck)
        {
            var uid = "2";
            var busStopIdS = busTable.BusStop.BusStopId.ToString();
            var toPad = 4 - busStopIdS.Length;
            for (int i = 0; i < toPad; i++)
            {
                uid += "0";
            }

            uid += busStopIdS;
            var url = $"https://online.bgnaplata.rs/publicapi/v1/announcement/announcement.php?station_uid={uid}&action=get_announcement_data";
            _logger.LogInformation("Checking " + url);
            // ReSharper disable once UnusedVariable
            var get = _client.GetAsync(url).ContinueWith(response =>handleResponse(response.Result,busTable,time));
        }

    }
     
}
