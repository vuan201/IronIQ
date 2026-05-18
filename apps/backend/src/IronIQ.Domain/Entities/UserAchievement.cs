namespace IronIQ.Domain.Entities;

public class UserAchievement
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AchievementId { get; private set; }
    public DateTime UnlockedAt { get; private set; }

    private UserAchievement() { }

    public static UserAchievement Create(Guid userId, Guid achievementId)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AchievementId = achievementId,
            UnlockedAt = DateTime.UtcNow,
        };
}
