using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Features.WorkoutSessions.Events;
using IronIQ.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IronIQ.Application.Features.AiCoach.EventHandlers;

public class AutoProgressionHandler(
    IWorkoutSessionRepository sessionRepository,
    IProgressionSuggestionRepository suggestionRepository,
    IUnitOfWork unitOfWork,
    ILogger<AutoProgressionHandler> logger)
    : INotificationHandler<WorkoutSessionCompletedEvent>
{
    public async Task Handle(WorkoutSessionCompletedEvent notification, CancellationToken ct)
    {
        try
        {
            var session = await sessionRepository.GetByIdAsync(notification.SessionId, ct);
            if (session is null) return;

            var since = DateTime.UtcNow.AddDays(-60);
            var recentSessions = await sessionRepository.GetCompletedByUserSinceAsync(notification.UserId, since, ct);

            var previousSessions = recentSessions
                .Where(s => s.Id != notification.SessionId)
                .OrderByDescending(s => s.CompletedAt)
                .ToList();

            var suggestions = new List<ProgressionSuggestion>();

            foreach (var exerciseLog in session.ExerciseLogs)
            {
                if (exerciseLog.Sets.Count == 0) continue;

                var maxWeight = exerciseLog.Sets
                    .Where(s => s.WeightKg.HasValue)
                    .Select(s => s.WeightKg!.Value)
                    .DefaultIfEmpty(0f)
                    .Max();

                if (maxWeight <= 0) continue;

                var prevSession = previousSessions
                    .FirstOrDefault(s => s.ExerciseLogs
                        .Any(l => l.ExerciseId == exerciseLog.ExerciseId && l.Sets.Count > 0));

                if (prevSession is null) continue;

                var prevLog = prevSession.ExerciseLogs
                    .First(l => l.ExerciseId == exerciseLog.ExerciseId);

                var prevMaxWeight = prevLog.Sets
                    .Where(s => s.WeightKg.HasValue)
                    .Select(s => s.WeightKg!.Value)
                    .DefaultIfEmpty(0f)
                    .Max();

                if (prevMaxWeight <= 0) continue;

                // Suggest if weight hasn't changed between the two sessions
                if (Math.Abs(maxWeight - prevMaxWeight) < 0.1f)
                {
                    suggestions.Add(ProgressionSuggestion.Create(
                        notification.UserId,
                        notification.SessionId,
                        exerciseLog.ExerciseId,
                        exerciseLog.Exercise?.Name ?? string.Empty,
                        maxWeight));
                }
            }

            if (suggestions.Count > 0)
            {
                await suggestionRepository.AddRangeAsync(suggestions, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AutoProgressionHandler failed for session {SessionId}", notification.SessionId);
        }
    }
}
