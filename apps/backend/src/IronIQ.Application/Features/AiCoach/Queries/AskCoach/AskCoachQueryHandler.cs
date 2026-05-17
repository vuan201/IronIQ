using System.Text;
using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using MediatR;

namespace IronIQ.Application.Features.AiCoach.Queries.AskCoach;

public class AskCoachQueryHandler(
    IAIService aiService,
    IUserRepository userRepository,
    IWorkoutSessionRepository sessionRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<AskCoachQuery, Result<CoachResponseDto>>
{
    public async Task<Result<CoachResponseDto>> Handle(AskCoachQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<CoachResponseDto>.Failure(Error.Unauthorized());

        var userId = currentUser.UserId.Value;
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
            return Result<CoachResponseDto>.Failure(Error.Unauthorized());

        var since = DateTime.UtcNow.AddDays(-30);
        var recentSessions = await sessionRepository.GetCompletedByUserSinceAsync(userId, since, ct);
        var last5 = recentSessions.OrderByDescending(s => s.CompletedAt).Take(5).ToList();

        var systemPrompt = BuildSystemPrompt(user, last5);

        var messages = query.History
            .Select(m => new ChatMessage(m.Role, m.Content))
            .Append(new ChatMessage("user", query.Message))
            .ToList();

        var reply = await aiService.ChatAsync(systemPrompt, messages, maxTokens: 1024, ct: ct);

        return Result<CoachResponseDto>.Success(new CoachResponseDto(reply));
    }

    private static string BuildSystemPrompt(Domain.Entities.User user, List<Domain.Entities.WorkoutSession> sessions)
    {
        var sb = new StringBuilder();
        sb.AppendLine("You are IronIQ, a knowledgeable and encouraging AI fitness coach.");
        sb.AppendLine("Respond concisely and practically. Keep answers under 200 words unless a detailed breakdown is needed.");
        sb.AppendLine();
        sb.AppendLine("## User Profile");
        var profile = user.Profile;
        if (!string.IsNullOrEmpty(profile.Name)) sb.AppendLine($"- Name: {profile.Name}");
        if (profile.Goal.HasValue) sb.AppendLine($"- Goal: {profile.Goal}");
        if (profile.Level.HasValue) sb.AppendLine($"- Fitness level: {profile.Level}");
        if (profile.WeightKg.HasValue) sb.AppendLine($"- Weight: {profile.WeightKg} kg");
        if (profile.HeightCm.HasValue) sb.AppendLine($"- Height: {profile.HeightCm} cm");

        if (sessions.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("## Recent Workouts (last 5 completed sessions)");
            foreach (var s in sessions)
            {
                var date = s.CompletedAt?.ToString("yyyy-MM-dd") ?? "unknown";
                var totalSets = s.ExerciseLogs.Sum(l => l.Sets.Count);
                var completedSets = s.ExerciseLogs.Sum(l => l.Sets.Count(set => set.IsCompleted));
                sb.AppendLine($"- {date}: {s.ExerciseLogs.Count} exercises, {completedSets}/{totalSets} sets completed");
            }
        }

        return sb.ToString();
    }
}
