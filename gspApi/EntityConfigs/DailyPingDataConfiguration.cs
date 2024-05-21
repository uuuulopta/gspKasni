namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DailyPingDataConfiguration: IEntityTypeConfiguration<DailyPingData>
{

    public void Configure(EntityTypeBuilder<DailyPingData> builder)
    {
        
        builder.HasKey(b => b.DailyPingDataId);
        builder.HasOne(b => b.BusRoute);
        builder.Property(b => b.BusRoute).IsRequired();
        builder.Property(e => e.Timestamp).IsRowVersion();                  
    }
}
