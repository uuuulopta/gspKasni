namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DayTypeConfiguration : IEntityTypeConfiguration<DayType>
{

    public void Configure(EntityTypeBuilder<DayType> builder)
    {
        builder.HasKey(m => m.DayTypeId);
        builder.HasIndex(d => d.Name).IsUnique();
        
        var workday = new DayType("Workday") { DayTypeId = 1 };
        var saturday = new DayType("Saturday") { DayTypeId = 2 };
        var sunday = new DayType("Sunday") { DayTypeId = 3 };
        builder.HasData(workday,saturday,sunday);
    }
}
