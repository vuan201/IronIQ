using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Progress.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Progress.Queries.GetProgress;

public class GetProgressQueryHandler(
    IWorkoutSessionRepository repository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetProgressQuery, Result<ProgressDto>>
{
    public async Task<Result<ProgressDto>> Handle(GetProgressQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<ProgressDto>.Failure(Error.Unauthorized());

        var since = DateTime.UtcNow.AddDays(-7 * query.WeeksBack);
        var sessions = await repository.GetCompletedByUserSinceAsync(currentUser.UserId.Value, since, ct);

        // Group by ISO week
        var weeklyGroups = sessions
            .GroupBy(s => GetWeekLabel(s.CompletedAt ?? s.StartedAt))
            .OrderBy(g => g.Key)
            .Select(g => new WeeklySessionCountDto(g.Key, g.Count()))
            .ToList();

        // Fill missing weeks with 0
        var allWeeks = Enumerable.Range(0, query.WeeksBack)
            .Select(i => GetWeekLabel(DateTime.UtcNow.AddDays(-7 * i)))
            .Reverse()
            .ToList();

        var weeklyCounts = allWeeks
            .Select(w => weeklyGroups.FirstOrDefault(g => g.WeekLabel == w) ?? new WeeklySessionCountDto(w, 0))
            .ToList();

        // Volume per session (last 10)
        var recentVolume = sessions
            .TakeLast(10)
            .Select(s =>
            {
                var volume = s.ExerciseLogs
                    .SelectMany(l => l.Sets)
                    .Where(set => set.WeightKg.HasValue && set.Reps.HasValue)
                    .Sum(set => (double)(set.WeightKg!.Value * set.Reps!.Value));

                var dateLabel = (s.CompletedAt ?? s.StartedAt).ToString("MM/dd");
                return new VolumePointDto(dateLabel, Math.Round(volume, 1));
            })
            .ToList();

        var totalSessions = sessions.Count;
        var totalSets = sessions.Sum(s => s.ExerciseLogs.Sum(l => l.Sets.Count));

        return Result<ProgressDto>.Success(new ProgressDto(weeklyCounts, recentVolume, totalSessions, totalSets));
    }

    private static string GetWeekLabel(DateTime dt)
    {
        // ISO week: Monday-based, format "Wxx"
        var monday = dt.AddDays(-(((int)dt.DayOfWeek + 6) % 7));
        return monday.ToString("MM/dd");
    }
}
