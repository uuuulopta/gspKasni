namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PingCacheConfiguration : IEntityTypeConfiguration<PingCache>
{

    public void Configure(EntityTypeBuilder<PingCache> builder)
    {
        builder.HasKey(b => b.PingCacheId);
        builder.HasOne(b => b.BusTable);
        builder.HasOne(b => b.Time);
        builder.Property(b => b.Lat).IsRequired();
        builder.Property(b => b.Lon).IsRequired();
        builder.Property(b => b.Distance).IsRequired();
        builder.Property(e => e.Timestamp).IsRowVersion();                  
    }
}
