using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Queries.GetMyPlans;

public class GetMyPlansQueryHandler(
    IWorkoutPlanRepository repository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetMyPlansQuery, Result<List<WorkoutPlanDto>>>
{
    public async Task<Result<List<WorkoutPlanDto>>> Handle(GetMyPlansQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<List<WorkoutPlanDto>>.Failure(Error.Unauthorized());

        var plans = await repository.GetByUserIdAsync(currentUser.UserId.Value, ct);
        return Result<List<WorkoutPlanDto>>.Success(plans.Select(MapToDto).ToList());
    }

    private static WorkoutPlanDto MapToDto(WorkoutPlan plan) => new(
        plan.Id,
        plan.Name,
        plan.Description,
        plan.IsActive,
        plan.CreatedAt,
        plan.Days
            .OrderBy(d => d.DayOfWeek)
            .Select(d => new WorkoutDayDto(
                d.Id,
                d.DayOfWeek,
                d.Name,
                d.Exercises
                    .OrderBy(e => e.Order)
                    .Select(e => new PlanExerciseDto(
                        e.Id, e.ExerciseId, e.Exercise?.Name ?? string.Empty,
                        e.Order, e.Sets, e.Reps, e.DurationSeconds, e.RestSeconds, e.Notes)
                    ).ToList()
            )).ToList());
}
