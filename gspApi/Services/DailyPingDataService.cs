namespace gspAPI.Services;

using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using Entities;

public class DailyPingDataService: IHostedService
{
    // ReSharper disable once NotAccessedField.Local
    Timer? _timer;
    readonly ILogger<DailyPingDataService> _logger;
    IBusTableRepository _repo;

    public DailyPingDataService(IServiceProvider services)
    {
        var scope = services.CreateScope();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<DailyPingDataService>>();
        _repo = scope.ServiceProvider.GetRequiredService<IBusTableRepository>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (await _repo.isTableEmptyAsync<DailyPingData>())
        {
            _logger.LogInformation("DailyPingData table is empty. Starting to fill it...");
            var date = _repo.getOldestPingCacheDate();
            var last = _repo.getNewestPingCacheDate();

            var count = 0;
            var diff = (last - date).TotalDays;
            while (date <= last)
            {
                await CreateDailyPingsForDate(date);
                
                _logger.LogInformation($"Seeded DailyPingData {++count} / {diff}");
                date = date.AddDays(1);
            }
        }

        _timer = new Timer(
            DoDaily,
            null,
            0,
            //1 day
            86400000
            
        );
        await Task.CompletedTask;
    }

    void DoDaily(object? param)
    {
        var date = DateTime.Now.AddDays(-1);
        var task = _repo.existsDailyPingDataForDate(date);
        task.Wait();
        if (!task.Result)
        {
            Task.WaitAll(CreateDailyPingsForDate(date));
        }
    }

    async Task CreateDailyPingsForDate(DateTime date)
    {
         
        int formattedDate = int.Parse(date.ToString("yyyyMMdd"));
        var data = await _repo.getPingCacheFormattedData(
            int.Parse(date.ToString("yyyyMMdd")),
            int.Parse(date.AddDays(1).ToString("yyyyMMdd"))
        );
        var pingData = data.ToList();
        if (pingData.Any())
        {
            List<DailyPingData> ldp = new();
            foreach (var pd in pingData)
            {
                var bt = await _repo.getBusTablesByName(pd.id);
                        
                ldp.Add(new DailyPingData
                {
                    BusRouteId =  bt.First().BusRouteId,
                    AvgDistance = pd.avg_distance,
                    AvgStationsBetween = pd.avg_stations_between,
                    Timestamp = date,
                    Score = pd.score
                }); 
            }

            await _repo.addDailyPingDataRangeAsync(ldp);
            await _repo.saveChangesAsync();
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
