using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.CreateWorkoutPlan;

public class CreateWorkoutPlanCommandHandler(
    IWorkoutPlanRepository repository,
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWorkoutPlanCommand, Result<WorkoutPlanDto>>
{
    public async Task<Result<WorkoutPlanDto>> Handle(CreateWorkoutPlanCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<WorkoutPlanDto>.Failure(Error.Unauthorized());

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null) return Result<WorkoutPlanDto>.Failure(Error.Unauthorized());

        if (user.SubscriptionTier == SubscriptionTier.Free)
        {
            var activeCount = await repository.CountActiveByUserIdAsync(currentUser.UserId.Value, ct);
            if (activeCount >= 2)
                return Result<WorkoutPlanDto>.Failure(Error.Conflict("Free tier allows up to 2 active plans. Upgrade to Premium for unlimited plans."));
        }

        var plan = WorkoutPlan.Create(currentUser.UserId.Value, command.Name, command.Description);

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

        await repository.AddAsync(plan, ct);
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
