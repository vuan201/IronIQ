namespace IronIQ.Domain.Entities;

public class PlanExercise
{
    public Guid Id { get; private set; }
    public Guid WorkoutDayId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public int Order { get; private set; }
    public int Sets { get; private set; }
    public int? Reps { get; private set; }
    public int? DurationSeconds { get; private set; }
    public int RestSeconds { get; private set; }
    public string? Notes { get; private set; }

    public Exercise? Exercise { get; private set; }

    private PlanExercise() { }

    public static PlanExercise Create(
        Guid dayId,
        Guid exerciseId,
        int order,
        int sets,
        int? reps,
        int? durationSeconds,
        int restSeconds,
        string? notes)
    {
        if (sets <= 0) throw new ArgumentException("Sets must be > 0.");
        return new PlanExercise
        {
            Id = Guid.NewGuid(),
            WorkoutDayId = dayId,
            ExerciseId = exerciseId,
            Order = order,
            Sets = sets,
            Reps = reps,
            DurationSeconds = durationSeconds,
            RestSeconds = restSeconds,
            Notes = notes?.Trim()
        };
    }
}
