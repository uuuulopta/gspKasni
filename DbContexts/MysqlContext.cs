﻿namespace gspAPI.DbContexts;

using Entities;
using EntityConfigs;
using Microsoft.EntityFrameworkCore;


public class MysqlContext: DbContext
{
    
    private readonly IWebHostEnvironment _env;
    public DbSet<BusTable> BusTables { get; set; } = null!;

    public DbSet<BusRoute> BusRoutes { get; set; } = null!;
    public DbSet<BusTrip> BusTrips { get; set; } = null!;
    public DbSet<BusStop> BusStops { get; set; } = null!;
    public MysqlContext(DbContextOptions<MysqlContext> options,IWebHostEnvironment environment): base(options)
    {
        _env = environment;
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
