using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.CreateWorkoutPlan;

public record CreateWorkoutPlanCommand(
    string Name,
    string? Description,
    List<CreateWorkoutDayDto> Days) : IRequest<Result<WorkoutPlanDto>>;

public record CreateWorkoutDayDto(
    int DayOfWeek,
    string? Name,
    List<CreatePlanExerciseDto> Exercises);

public record CreatePlanExerciseDto(
    Guid ExerciseId,
    int Order,
    int Sets,
    int? Reps,
    int? DurationSeconds,
    int RestSeconds,
    string? Notes);
