using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.UpdateWorkoutPlan;

public class UpdateWorkoutPlanCommandHandler(
    IWorkoutPlanRepository repository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateWorkoutPlanCommand, Result<WorkoutPlanDto>>
{
    public async Task<Result<WorkoutPlanDto>> Handle(UpdateWorkoutPlanCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<WorkoutPlanDto>.Failure(Error.Unauthorized());

        var plan = await repository.GetByIdAsync(command.PlanId, ct);
        if (plan is null)
            return Result<WorkoutPlanDto>.Failure(Error.NotFound("WorkoutPlan", command.PlanId));

        if (plan.UserId != currentUser.UserId.Value)
            return Result<WorkoutPlanDto>.Failure(Error.Unauthorized("You do not own this plan."));

        plan.Update(command.Name, command.Description);

        var days = command.Days.Select(d =>
        {
            var day = WorkoutDay.Create(plan.Id, d.DayOfWeek, d.Name);
            var exercises = d.Exercises.Select(e =>
                PlanExercise.Create(day.Id, e.ExerciseId, e.Order, e.Sets, e.Reps, e.DurationSeconds, e.RestSeconds, e.Notes)
            ).ToList();
            day.Exercises.AddRange(exercises);
            return day;
        }).ToList();

        plan.ReplaceDays(days);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<WorkoutPlanDto>.Success(MapToDto(plan));
    }

    private static WorkoutPlanDto MapToDto(WorkoutPlan plan) => new(
        plan.Id,
        plan.Name,
        plan.Description,
        plan.IsActive,
        plan.CreatedAt,
        plan.Days.Select(d => new WorkoutDayDto(
            d.Id,
            d.DayOfWeek,
            d.Name,
            d.Exercises.Select(e => new PlanExerciseDto(
                e.Id, e.ExerciseId, e.Exercise?.Name ?? string.Empty,
                e.Order, e.Sets, e.Reps, e.DurationSeconds, e.RestSeconds, e.Notes)
            ).ToList()
        )).ToList());
}
