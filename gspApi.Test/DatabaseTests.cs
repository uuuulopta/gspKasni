namespace gspApi.Test;

using Castle.Core.Internal;
using gspAPI.BusTableAPI;
using gspAPI.DbContexts;
using gspAPI.Entities;
using gspAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic.FileIO;
using Xunit.Abstractions;

public class DatabaseTests
{
    readonly ITestOutputHelper _testOutputHelper;

    public DatabaseTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    
    public static MysqlContext getDbContext(int busTableCount)
    {
        var options = getDbContextOptions();
        var dbContext = new MysqlContext(options);
        
        
        dbContext.Database.EnsureDeleted();
        Directory.SetCurrentDirectory("../../../../gspAPI");
        dbContext.Database.Migrate();
        

        dbContext.SaveChanges();
        return dbContext;

    }

    public static string getParentFolder(int levels=1,string? path = null)
    {
        if (path == null) path = Directory.GetCurrentDirectory();
        if (levels == 0) return path;
        return getParentFolder(levels - 1,
            Directory.GetParent(path)!.ToString());
    }
    public static DbContextOptions<MysqlContext> getDbContextOptions()
    {
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(getParentFolder(3))
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        IConfiguration config = builder.Build();
        var connString = config.GetConnectionString("TestDatabase");
        return new DbContextOptionsBuilder<MysqlContext>()
            .UseMySql(connString ?? throw new ArgumentNullException(),ServerVersion.AutoDetect(connString))
            .Options;
    }

    public static IBusTableRepository getRepository(MysqlContext context)
    {
        return new BusTableRepository(context);
    }
    [Fact]
    public async void testUpdateTable()
    {

        _testOutputHelper.WriteLine(getParentFolder(3));
        
        var context = getDbContext(0);
        var repository = getRepository(context);
        var getter = new BusTableGetter(repository, NullLogger<BusTableGetter>.Instance);
        await getter.getBusTableFromWebAndCache("2");
        var tbCount = context.TimeBusTables.Count();
        var btCount = context.BusTables.Count();
        var timeCount = context.Times.Count(); 
        
        context.ChangeTracker.Clear();
        context.Times.Add(new Time() { TimeId = 9999, Hour = 99, Minute = 99, DayTypeId = 3 });
        context.TimeBusTables.Add(new TimeBusTable() { BusTableId = 1, TimeId = 9999 });
        context.BusTables.ExecuteUpdate(t => t.SetProperty(b => b.LastUpdated, "2020-01-01"));
        await context.SaveChangesAsync();
        
        context.ChangeTracker.Clear();
        await getter.getBusTableFromWebAndCache("2");

        var tbCount2 = context.TimeBusTables.Count();
        var btCount2 = context.BusTables.Count();
        var timeCount2 = context.Times.Count(); 
        
        Assert.True(tbCount == tbCount2);
        Assert.True(btCount == btCount2);
        Assert.True(timeCount+1 == timeCount2);
    }
}
  