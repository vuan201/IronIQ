using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.DeleteWorkoutPlan;

public class DeleteWorkoutPlanCommandHandler(
    IWorkoutPlanRepository repository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteWorkoutPlanCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkoutPlanCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result.Failure(Error.Unauthorized());

        var plan = await repository.GetByIdAsync(command.PlanId, ct);
        if (plan is null)
            return Result.Failure(Error.NotFound("WorkoutPlan", command.PlanId));

        if (plan.UserId != currentUser.UserId.Value)
            return Result.Failure(Error.Unauthorized("You do not own this plan."));

        await repository.DeleteAsync(plan, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
