namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BusRouteConfiguration : IEntityTypeConfiguration<BusRoute>
{

    public void Configure(EntityTypeBuilder<BusRoute> builder)
    {
        builder.HasKey(b => b.BusRouteId);
        builder.Property(b => b.NameShort).IsRequired();
        builder.Property(b => b.NameLong).IsRequired();
    }
}
