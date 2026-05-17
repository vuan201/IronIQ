namespace IronIQ.Application.Features.WorkoutSessions.DTOs;

public record WorkoutSessionDto(
    Guid Id,
    Guid? WorkoutPlanId,
    Guid? WorkoutDayId,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,
    string? Notes,
    List<ExerciseLogDto> ExerciseLogs);

public record ExerciseLogDto(
    Guid Id,
    Guid ExerciseId,
    string ExerciseName,
    int Order,
    List<SetLogDto> Sets);

public record SetLogDto(
    Guid Id,
    int SetNumber,
    float? WeightKg,
    int? Reps,
    int? DurationSeconds,
    bool IsCompleted);

public record WorkoutSessionSummaryDto(
    Guid Id,
    Guid? WorkoutPlanId,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,
    int TotalExercises,
    int TotalSets);

public record SessionHistoryPageDto(
    IList<WorkoutSessionSummaryDto> Items,
    int Total,
    int Page,
    int PageSize);
