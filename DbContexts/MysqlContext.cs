namespace gspAPI.DbContexts;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        modelBuilder.Entity<Minute>().HasKey(m => m.MinuteId);
        modelBuilder.Entity<Minute>().Property(m => m.Value).IsRequired();
        modelBuilder.Entity<Minute>()
            .HasOne(m => m.DayType)
            .WithMany(m => m.Minutes)
            .HasForeignKey(m => m.DayTypeId);

        modelBuilder.Entity<BusTable>().HasKey(b => b.BusTableId);
        modelBuilder.Entity<BusTable>()
            .HasIndex(b => b.LineNumber)
            .IsUnique();
        modelBuilder.Entity<BusTable>()
            .HasMany(b => b.Minutes)
            .WithMany(b => b.BusTables);
        // modelBuilder.Entity<BusTable>().Property(b => b.WorkdayArrivals).HasConversion(intListToString).Metadata.SetValueComparer(arrivalComparer);



        var workday = new DayType("Workday") { DayTypeId = 1 };
        var saturday = new DayType("Saturday") { DayTypeId = 2 };
        var sunday = new DayType("Sunday") { DayTypeId = 3 };
        modelBuilder.Entity<DayType>().HasData(workday,saturday,sunday);
        if (_env.IsDevelopment())
        {
            // TODO: seed
        }
    }

  
}
