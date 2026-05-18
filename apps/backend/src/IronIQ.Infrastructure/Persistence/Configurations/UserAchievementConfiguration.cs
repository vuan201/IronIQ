using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronIQ.Infrastructure.Persistence.Configurations;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        builder.HasKey(ua => ua.Id);
        builder.HasIndex(ua => new { ua.UserId, ua.AchievementId }).IsUnique();
        builder.HasIndex(ua => ua.UserId);
    }
}
