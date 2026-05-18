using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Features.WorkoutSessions.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IronIQ.Application.Features.Social.EventHandlers;

public class UpdateStreakHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateStreakHandler> logger)
    : INotificationHandler<WorkoutSessionCompletedEvent>
{
    public async Task Handle(WorkoutSessionCompletedEvent notification, CancellationToken ct)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(notification.UserId, ct);
            if (user is null) return;

            user.UpdateStreak(DateOnly.FromDateTime(DateTime.UtcNow));
            await unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateStreakHandler failed for user {UserId}", notification.UserId);
        }
    }
}
