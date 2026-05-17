using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Queries.GetSessionHistory;

public class GetSessionHistoryQueryHandler(
    IWorkoutSessionRepository repository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetSessionHistoryQuery, Result<SessionHistoryPageDto>>
{
    public async Task<Result<SessionHistoryPageDto>> Handle(GetSessionHistoryQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<SessionHistoryPageDto>.Failure(Error.Unauthorized());

        var (items, total) = await repository.GetByUserIdAsync(currentUser.UserId.Value, query.Page, query.PageSize, ct);

        var dtos = items.Select(s => new WorkoutSessionSummaryDto(
            s.Id,
            s.WorkoutPlanId,
            s.StartedAt,
            s.CompletedAt,
            s.Status.ToString(),
            s.ExerciseLogs.Count,
            s.ExerciseLogs.Sum(l => l.Sets.Count)
        )).ToList();

        return Result<SessionHistoryPageDto>.Success(new SessionHistoryPageDto(dtos, total, query.Page, query.PageSize));
    }
}
