namespace gspAPI.DbContexts;

using Entities;
using EntityConfigs;
using Microsoft.EntityFrameworkCore;
using Models;

public class MysqlContext: DbContext
{
    
    public DbSet<Time> Times { get; set; } = null!;
    public DbSet<BusTable> BusTables { get; set; } = null!;

    public DbSet<BusRoute> BusRoutes { get; set; } = null!;
    public DbSet<BusTrip> BusTrips { get; set; } = null!;
    public DbSet<BusStop> BusStops { get; set; } = null!;
    public DbSet<BusTripBusStop> BusTripBusStops { get; set; } = null!;
    public DbSet<PingCache> PingCaches { get; set; } = null!;
    public DbSet<DayType> DayTypes { get; set; } = null!;
    public DbSet<TimeBusTable> TimeBusTables { get; set; } = null!;
    public DbSet<PingData> PingData { get; set; } = null!;
    public MysqlContext(DbContextOptions<MysqlContext> options): base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new BusRouteConfiguration().Configure(modelBuilder.Entity<BusRoute>());
        new BusStopConfiguration().Configure(modelBuilder.Entity<BusStop>());
        new BusTableConfiguration().Configure(modelBuilder.Entity<BusTable>());
        new BusTripConfiguration().Configure(modelBuilder.Entity<BusTrip>());
        new DayTypeConfiguration().Configure(modelBuilder.Entity<DayType>());
        new PingCacheConfiguration().Configure(modelBuilder.Entity<PingCache>());
        new TimeConfiguration().Configure(modelBuilder.Entity<Time>());

        // Seed database with route, trip and stop data if it has not already been done so.
       
    //     if (_env.IsDevelopment() && false)
    //     {
    //         var fakeData = new FakeDataGenerator();
    //         fakeData.newData(20,1000);
    //         modelBuilder.Entity<Time>().HasData(fakeData.Times);
    //         modelBuilder.Entity<BusTable>().HasData(fakeData.BusTables);
    //         modelBuilder.Entity<TimeBusTable>().HasData(fakeData.MinuteBusTables);
    //     }
     }

  
}
