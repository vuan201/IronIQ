using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronIQ.Infrastructure.Persistence.Configurations;

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Key).IsRequired().HasMaxLength(50);
        builder.HasIndex(a => a.Key).IsUnique();
        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(300);
        builder.Property(a => a.IconEmoji).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Type).HasConversion<string>();
    }
}
