using System.Text;
using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.AiCoach.Queries.GetSessionReview;

public class GetSessionReviewQueryHandler(
    IAIService aiService,
    IWorkoutSessionRepository sessionRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetSessionReviewQuery, Result<SessionReviewDto>>
{
    public async Task<Result<SessionReviewDto>> Handle(GetSessionReviewQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<SessionReviewDto>.Failure(Error.Unauthorized());

        var userId = currentUser.UserId.Value;
        var session = await sessionRepository.GetByIdAsync(query.SessionId, ct);
        if (session is null || session.UserId != userId)
            return Result<SessionReviewDto>.Failure(Error.NotFound("Session", query.SessionId));

        var since = DateTime.UtcNow.AddDays(-90);
        var recentSessions = await sessionRepository.GetCompletedByUserSinceAsync(userId, since, ct);

        var previousSession = recentSessions
            .Where(s => s.Id != query.SessionId && s.CompletedAt < session.CompletedAt)
            .OrderByDescending(s => s.CompletedAt)
            .FirstOrDefault();

        var prompt = BuildPrompt(session, previousSession);
        var review = await aiService.CompleteAsync(prompt, maxTokens: 400, ct: ct);

        return Result<SessionReviewDto>.Success(new SessionReviewDto(review));
    }

    private static string BuildPrompt(WorkoutSession current, WorkoutSession? previous)
    {
        var sb = new StringBuilder();
        sb.AppendLine("You are IronIQ, an encouraging AI fitness coach.");
        sb.AppendLine("Write a 2-3 sentence workout review based on the session data below.");
        sb.AppendLine("Be specific with numbers. Note improvements or consistency vs the previous session if available.");
        sb.AppendLine("End with one actionable tip for next time.");
        sb.AppendLine("Write only the review text — no headers, no bullet points, no preamble.");
        sb.AppendLine();

        sb.AppendLine($"## Today's session ({current.CompletedAt?.ToString("yyyy-MM-dd") ?? "today"})");
        foreach (var log in current.ExerciseLogs)
        {
            var name = log.Exercise?.Name ?? log.ExerciseId.ToString();
            var sets = log.Sets.Count;
            var maxWeight = log.Sets.Where(s => s.WeightKg.HasValue).Select(s => s.WeightKg!.Value).DefaultIfEmpty(0f).Max();
            var totalReps = log.Sets.Where(s => s.Reps.HasValue).Sum(s => s.Reps!.Value);
            sb.AppendLine(maxWeight > 0
                ? $"- {name}: {sets} sets, max {maxWeight}kg, {totalReps} total reps"
                : $"- {name}: {sets} sets, {totalReps} total reps");
        }

        if (previous is not null)
        {
            sb.AppendLine();
            sb.AppendLine($"## Previous session ({previous.CompletedAt?.ToString("yyyy-MM-dd") ?? "unknown"})");
            foreach (var log in previous.ExerciseLogs)
            {
                var name = log.Exercise?.Name ?? log.ExerciseId.ToString();
                var sets = log.Sets.Count;
                var maxWeight = log.Sets.Where(s => s.WeightKg.HasValue).Select(s => s.WeightKg!.Value).DefaultIfEmpty(0f).Max();
                var totalReps = log.Sets.Where(s => s.Reps.HasValue).Sum(s => s.Reps!.Value);
                sb.AppendLine(maxWeight > 0
                    ? $"- {name}: {sets} sets, max {maxWeight}kg, {totalReps} total reps"
                    : $"- {name}: {sets} sets, {totalReps} total reps");
            }
        }

        return sb.ToString();
    }
}
