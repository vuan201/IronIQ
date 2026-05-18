using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Features.WorkoutSessions.Events;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IronIQ.Application.Features.Social.EventHandlers;

public class CheckAchievementsHandler(
    IUserRepository userRepository,
    IAchievementRepository achievementRepository,
    IUserAchievementRepository userAchievementRepository,
    IWorkoutSessionRepository sessionRepository,
    IUnitOfWork unitOfWork,
    ILogger<CheckAchievementsHandler> logger)
    : INotificationHandler<WorkoutSessionCompletedEvent>
{
    public async Task Handle(WorkoutSessionCompletedEvent notification, CancellationToken ct)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(notification.UserId, ct);
            if (user is null) return;

            var allAchievements = await achievementRepository.GetAllAsync(ct);
            var alreadyUnlocked = await userAchievementRepository.GetByUserIdAsync(notification.UserId, ct);
            var unlockedIds = alreadyUnlocked.Select(u => u.AchievementId).ToHashSet();

            var candidates = allAchievements.Where(a => !unlockedIds.Contains(a.Id)).ToList();
            if (candidates.Count == 0) return;

            var totalSessions = await sessionRepository.CountCompletedByUserAsync(notification.UserId, ct);
            var totalSets = await sessionRepository.CountCompletedSetsByUserAsync(notification.UserId, ct);
            var longestStreak = user.LongestStreak;

            var newUnlocks = new List<UserAchievement>();
            foreach (var achievement in candidates)
            {
                var met = achievement.Type switch
                {
                    AchievementType.Sessions => totalSessions >= achievement.Threshold,
                    AchievementType.Streak => longestStreak >= achievement.Threshold,
                    AchievementType.Sets => totalSets >= achievement.Threshold,
                    _ => false,
                };

                if (met)
                    newUnlocks.Add(UserAchievement.Create(notification.UserId, achievement.Id));
            }

            if (newUnlocks.Count > 0)
            {
                await userAchievementRepository.AddRangeAsync(newUnlocks, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CheckAchievementsHandler failed for user {UserId}", notification.UserId);
        }
    }
}
