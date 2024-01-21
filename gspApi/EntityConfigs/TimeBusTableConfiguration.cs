namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TimeBusTableConfiguration: IEntityTypeConfiguration<TimeBusTable>
{

    public void Configure(EntityTypeBuilder<TimeBusTable> builder)
    {
        builder.HasIndex(t => t.BusTableId);
        builder.HasIndex(t => t.TimeId);
    }
}
