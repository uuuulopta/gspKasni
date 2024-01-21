namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BusTableConfiguration : IEntityTypeConfiguration<BusTable>
{

    public void Configure(EntityTypeBuilder<BusTable> builder)
    {
        
        builder.HasKey(b => b.BusTableId);
        builder.HasIndex(b => b.BusRouteId);
        builder.HasIndex(b => b.BusStopId);
        builder.Property(b => b.Direction).IsRequired();
        builder
            .Property(b => b.LastUpdated)
            .IsRequired();
        builder
            .HasMany(b => b.Times)
            .WithMany(b => b.BusTables)
            .UsingEntity<TimeBusTable>();
        builder
            .HasOne(b => b.BusStop);
        builder.HasOne(b => b.BusRoute);
    }
}
