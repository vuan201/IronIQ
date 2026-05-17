namespace IronIQ.Domain.Entities;

public class SetLog
{
    public Guid Id { get; private set; }
    public Guid ExerciseLogId { get; private set; }
    public int SetNumber { get; private set; }
    public float? WeightKg { get; private set; }
    public int? Reps { get; private set; }
    public int? DurationSeconds { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private SetLog() { }

    public static SetLog Create(
        Guid exerciseLogId,
        int setNumber,
        float? weightKg,
        int? reps,
        int? durationSeconds)
    {
        return new SetLog
        {
            Id = Guid.NewGuid(),
            ExerciseLogId = exerciseLogId,
            SetNumber = setNumber,
            WeightKg = weightKg,
            Reps = reps,
            DurationSeconds = durationSeconds,
            IsCompleted = true,
            CompletedAt = DateTime.UtcNow
        };
    }
}
