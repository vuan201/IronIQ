namespace IronIQ.Domain.Entities;

public class ProgressionSuggestion
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid SessionId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public string ExerciseName { get; private set; } = "";
    public float CurrentWeightKg { get; private set; }
    public float SuggestedWeightKg { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ProgressionSuggestion() { }

    public static ProgressionSuggestion Create(
        Guid userId,
        Guid sessionId,
        Guid exerciseId,
        string exerciseName,
        float currentWeightKg)
    {
        return new ProgressionSuggestion
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            SessionId = sessionId,
            ExerciseId = exerciseId,
            ExerciseName = exerciseName,
            CurrentWeightKg = currentWeightKg,
            SuggestedWeightKg = currentWeightKg + 2.5f,
            CreatedAt = DateTime.UtcNow
        };
    }
}
