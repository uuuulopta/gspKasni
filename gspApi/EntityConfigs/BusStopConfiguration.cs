namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BusStopConfiguration : IEntityTypeConfiguration<BusStop>
{

    public void Configure(EntityTypeBuilder<BusStop> builder)
    {
        
        builder.HasKey(b => b.BusStopId);
        builder.Property(b => b.Lon).IsRequired();
        builder.Property(b => b.Lat).IsRequired();
        builder.Property(b => b.BusStopName).IsRequired();
        builder
            .HasMany(b => b.BusTrips)
            .WithMany(b => b.BusStops)
            .UsingEntity<BusTripBusStop>().HasKey(b => b.BusTripBusStopId);
    }
}
