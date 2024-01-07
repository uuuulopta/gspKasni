namespace gspApi.Test;

using Castle.Core.Internal;
using gspAPI.DbContexts;
using gspAPI.Entities;
using gspAPI.Migrations;
using gspAPI.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

public class EmptyDatabaseRepositoryTests
{
    readonly ITestOutputHelper _testOutputHelper;

    private static MysqlContext getDbContext()
    {
        var options = new DbContextOptionsBuilder<MysqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var dbContext = new MysqlContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;

    }

    private static IBusTableRepository getRepository()
    {
        return new BusTableRepository(getDbContext());
    }

    private IBusTableRepository repository = getRepository();

    public EmptyDatabaseRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void getCountBusTablesTest()
    {
        var count = repository.getCountBusTables();
        Assert.Equal(0,count); 
    }
    // public Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber);
    
    [Fact]
    public async void getBusTablesByNameTest()
    {
        var res = await repository.getBusTablesByName("doesnt matter since db is empty");
        Assert.True(res.ToList().Count == 0); 
    }
    // public void deleteBusTablesByName(string lineNumber);
    
   
    // public void deleteBusTablesByCollection(ICollection<BusTable> busTables);
    
  
    // public Task<(BusStop busStop, BusRoute busRoute)> getBusTableForeignsAsync(string tripLongName, int direction);
    
    [Fact]
    public async void getBusTableForeignsAsyncTest()
    {
        var (a, b) = await repository.getBusTableForeignsAsync("bla bla",1);
        Assert.Null(a);
        Assert.Null(b);
    }
    // public void addBusTable(BusTable bt);
        
    // public Task addBusTableRangeAsync(IEnumerable<BusTable> bt);
        
    // public Task<Dictionary<BusStop, List<BusTable>>> getBusTablesByTime(Time time);
    
    [Fact]
    public async void getBusTablesByTimeTest()
    {
        var daytype = new DayType("Workday") { DayTypeId = 1 };
        var time = new Time()
        {
            DayType = daytype,
            Hour = 1,
            Minute = 1,
            TimeId = 1
        };
        var res = await repository.getBusTablesByTime(time);
        Assert.True(res.Count == 0);
    }
    // public Task<List<string>> getAllRoutesShortNames();
    // public Task<bool> saveChangesAsync();
    // public Task<Time> getTime(int hour, int minute, int daytypeId);
    // public void addPingCache(PingCache pingCache);
    // public Task<BusTable?> getOppositeDirectionBusTable(BusTable bt);
    //
    // public Task<IEnumerable<PingData>> getPingCacheFormattedData();
    //
    // public Task<IEnumerable<LatestPingData>>? getLatestPings();
    // public Task<Time> getTimeCreateIfNone(int daytypeid, int hour, int minute);
    // public int getCountBusTables();
    // public void attach<T>(T target);
}
