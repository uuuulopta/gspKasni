namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BusTripConfiguration : IEntityTypeConfiguration<BusTrip>
{

    public void Configure(EntityTypeBuilder<BusTrip> builder)
    {
        builder.HasKey(b => b.BusTripId);
        builder.HasOne(b => b.BusRoute);
        builder.Property(b => b.BusTripDirection).IsRequired();
        builder.Property(b => b.BusTripName).IsRequired();


    }
}
