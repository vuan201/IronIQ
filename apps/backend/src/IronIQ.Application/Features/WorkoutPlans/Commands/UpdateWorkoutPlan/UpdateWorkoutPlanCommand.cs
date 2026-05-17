using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.Commands.CreateWorkoutPlan;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.UpdateWorkoutPlan;

public record UpdateWorkoutPlanCommand(
    Guid PlanId,
    string Name,
    string? Description,
    List<CreateWorkoutDayDto> Days) : IRequest<Result<WorkoutPlanDto>>;
