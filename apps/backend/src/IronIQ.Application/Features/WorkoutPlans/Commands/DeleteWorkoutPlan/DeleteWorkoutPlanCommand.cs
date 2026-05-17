using IronIQ.Application.Common.Models;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.DeleteWorkoutPlan;

public record DeleteWorkoutPlanCommand(Guid PlanId) : IRequest<Result>;
