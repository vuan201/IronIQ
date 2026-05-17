namespace IronIQ.Application.Features.AiCoach.DTOs;

public record CoachMessageDto(string Role, string Content);

public record CoachResponseDto(string Reply);

public record ProgressionSuggestionDto(
    Guid ExerciseId,
    string ExerciseName,
    float CurrentWeightKg,
    float SuggestedWeightKg);

public record SessionReviewDto(string Review);
