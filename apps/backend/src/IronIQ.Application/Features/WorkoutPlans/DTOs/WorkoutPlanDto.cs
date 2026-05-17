namespace IronIQ.Application.Features.WorkoutPlans.DTOs;

public record WorkoutPlanDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    List<WorkoutDayDto> Days);

public record WorkoutDayDto(
    Guid Id,
    int DayOfWeek,
    string? Name,
    List<PlanExerciseDto> Exercises);

public record PlanExerciseDto(
    Guid Id,
    Guid ExerciseId,
    string ExerciseName,
    int Order,
    int Sets,
    int? Reps,
    int? DurationSeconds,
    int RestSeconds,
    string? Notes);
