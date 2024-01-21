namespace gspApi.Test;

using FakeItEasy;
using gspAPI.DbContexts;
using gspAPI.Entities;
using gspAPI.Mappings;
using gspAPI.Migrations;
using gspAPI.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

public class DatabaseRepositoryTests
{
    readonly ITestOutputHelper _testOutputHelper;

    public DatabaseRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    public static void addDataElements(MysqlContext dbContext,int count)
    {
        if (count == 0) return;
        for (int i = 1; i <= count; i++)
        {
            dbContext.Times.Add(new Time(){ DayTypeId = 1, Hour = i, Minute = i });
            dbContext.BusStops.Add(new BusStop(){ BusStopName = $"stopname{i}", Lat = i, Lon = i } );
            dbContext.BusRoutes.Add(new BusRoute(){ NameShort = $"nameshort{i}", NameLong = $"namelong{i}" } );
            dbContext.BusTrips.Add(new BusTrip(){ BusRouteId = i,BusTripDirection = 0,BusTripName = $"tripname{i}",});
            dbContext.BusTripBusStops.Add(new BusTripBusStop(){ BusStopId = i, BusTripId = i, Direction = 0 } );
            dbContext.BusTables.Add(new BusTable() { BusRouteId = i, BusStopId = i, Direction = 0 });
            dbContext.TimeBusTables.Add(new TimeBusTable() { TimeId = i, BusTableId = i });
        }

        dbContext.SaveChanges();
    }

    public static MysqlContext getDbContext(int busTableCount)
    {
        var options = getDbContextOptions();
        var dbContext = new MysqlContext(options);
        dbContext.Database.EnsureCreated();
        addDataElements(dbContext,busTableCount);
        dbContext.SaveChanges();
        return dbContext;

    }

    public static DbContextOptions<MysqlContext> getDbContextOptions()
    {
        return new DbContextOptionsBuilder<MysqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    public static IBusTableRepository getRepository(MysqlContext context)
    {
        return new BusTableRepository(context);
    }





    static void dbCountTest(int count,MysqlContext context)
    {
        Assert.Equal(3,context.DayTypes.Count()); 
        Assert.Equal(count,context.BusTables.Count());
        Assert.Equal(count,context.BusRoutes.Count());
        Assert.Equal(count,context.BusStops.Count());
        Assert.Equal(count,context.Times.Count());
        Assert.Equal(count,context.BusTrips.Count());
        Assert.Equal(count,context.BusTripBusStops.Count());
        Assert.Equal(count,context.TimeBusTables.Count());
    }

    [Fact]
    public void initialTestEnviromentTest()
    {
        var context = getDbContext(1);
        dbCountTest(1,context);
    }


// public Task<IEnumerable<BusTable>> getBusTablesByName(string lineNumber);
    [Fact]
    public async void getBusTablesByNameTest()
    {
        var context = getDbContext(1);
        var repository = getRepository(context);
        var res = (await repository.getBusTablesByName("nameshort1")).ToList();
        Assert.Single(res);
        var bt = res[0];
        var times = bt.Times.ToList();
        Assert.Single(times);
        var time = times[0];
        Assert.True(time.Hour == 1 && time.Minute == 1 && time.TimeId == 1);
        Assert.True(bt.BusStop.BusStopName == "stopname1" && bt.BusStop.BusStopId == 1);
        Assert.True(bt.BusRoute.NameShort == "nameshort1" && bt.BusRouteId == 1);
        

    }
//     public void deleteBusTablesByName(string lineNumber);
  
//     public Task<(BusStop? busStop, BusRoute? busRoute)> getBusTableForeignsAsync(string tripLongName, int direction);
    [Fact]
    public async void setBusTableForeignsAsyncTest()
    {
        var context = getDbContext(1);
        var repository = getRepository(context);
        (var bs, var br) = await repository.getBusTableForeignsAsync("nameshort1",0);
        Assert.NotNull(bs);
        Assert.NotNull(br);
        Assert.True(bs.BusStopName == "stopname1" && bs.BusStopId == 1);
        Assert.True(br.NameShort == "nameshort1" && br.BusRouteId == 1);
    }
    // public Task<Dictionary<BusStop, List<BusTable>>> getBusTablesByTime(Time time);
//     public Task<Time> getTime(int hour, int minute, int daytypeId);
    [Fact]
    public async void getBusTablesByTimeParamsTest()
    {
        var context = getDbContext(3);
        var repository = getRepository(context);
        var res = await repository.getBusTablesByTime(2, 2, 1);
        Assert.Single(res.Keys);
        var bs = res.Keys.FirstOrDefault()!;
        Assert.True(bs.BusStopName == "stopname2" && bs.BusStopId == 2);
        Assert.Single(res.Values);
        var btList = res.Values.FirstOrDefault()!;
        Assert.NotEmpty(btList);
        var bt = btList.FirstOrDefault();
        Assert.True(bt!.BusStopId== 2);
    }
    // public Task<Time> getTimeCreateIfNone(int daytypeid, int hour, int minute);
    [Fact]
    public async void getTimeCreateIfNoneTest()
    {
        var context = getDbContext(1);
        var repository = getRepository(context);
        var time = await repository.getTimeCreateIfNone(1,1,1);
        Assert.True(time.TimeId == 1 && time.Hour == 1 && time.Minute == 1);
        Assert.Equal(1,context.Times.Count());
        var timeCreated = await repository.getTimeCreateIfNone(2, 2, 2);
        Assert.True(timeCreated.DayTypeId == 2 && timeCreated.Hour == 2 && timeCreated.Minute == 2);
    }


  
    // public void addBusTable(BusTable bt);
//     public Task addBusTableRangeAsync(IEnumerable<BusTable> bt);
//     public Task<List<string>> getAllRoutesShortNames();
//     public void addPingCache(PingCache pingCache);
//     public Task<BusTable?> getOppositeDirectionBusTable(BusTable bt);
//
//     public Task<IEnumerable<PingData>> getPingCacheFormattedData();
//
//     public Task<IEnumerable<LatestPingData>>? getLatestPings();
//     public int getCountBusTables();

}
