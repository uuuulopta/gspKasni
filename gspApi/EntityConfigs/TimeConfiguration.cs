namespace gspAPI.EntityConfigs;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TimeConfiguration : IEntityTypeConfiguration<Time>
{

    public void Configure(EntityTypeBuilder<Time> builder)
    {
        builder.HasKey(m => m.TimeId);
        builder.Property(m => m.Minute).IsRequired();
        builder
            .HasOne(m => m.DayType)
            .WithMany(m => m.Minutes)
            .HasForeignKey(m => m.DayTypeId);
    }
}
