namespace gspAPI.DbContexts;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class MysqlContext: DbContext
{
    
    private readonly IWebHostEnvironment _env;
    public DbSet<BusTable> BusTables { get; set; } = null!;
    public MysqlContext(DbContextOptions<MysqlContext> options,IWebHostEnvironment environment): base(options)
    {
        _env = environment;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       

        modelBuilder.Entity<DayType>().HasKey(m => m.DayTypeId);
        modelBuilder.Entity<DayType>().HasIndex(d => d.Name).IsUnique();

        modelBuilder.Entity<Time>().HasKey(m => m.TimeId);
        modelBuilder.Entity<Time>().Property(m => m.Minute).IsRequired();
        modelBuilder.Entity<Time>()
            .HasOne(m => m.DayType)
            .WithMany(m => m.Minutes)
            .HasForeignKey(m => m.DayTypeId);

        modelBuilder.Entity<BusTable>().HasKey(b => b.BusTableId);

        modelBuilder.Entity<BusTable>()
            .Property(b => b.LastUpdated)
            .IsRequired();
        modelBuilder.Entity<BusTable>()
            .HasMany(b => b.Times)
            .WithMany(b => b.BusTables)
            .UsingEntity<TimeBusTable>();
        modelBuilder.Entity<BusTable>(). Property(b => b.Direction).IsRequired();



        var workday = new DayType("Workday") { DayTypeId = 1 };
        var saturday = new DayType("Saturday") { DayTypeId = 2 };
        var sunday = new DayType("Sunday") { DayTypeId = 3 };
        modelBuilder.Entity<DayType>().HasData(workday,saturday,sunday);
        if (_env.IsDevelopment())
        {
            var fakeData = new FakeDataGenerator();
            fakeData.newData(20,4);
            modelBuilder.Entity<Time>().HasData(fakeData.Times);
            modelBuilder.Entity<BusTable>().HasData(fakeData.BusTables);
            modelBuilder.Entity<TimeBusTable>().HasData(fakeData.MinuteBusTables);
        }
    }

  
}
