namespace IronIQ.Application.Features.Exercises.DTOs;

public record ExerciseDto(
    Guid Id,
    string Name,
    string? Description,
    List<string> MuscleGroups,
    List<string> Equipment,
    string Difficulty,
    bool IsSystem,
    Guid? CreatedByUserId);

public record ExercisesPageDto(IList<ExerciseDto> Items, int Total, int Page, int PageSize);
