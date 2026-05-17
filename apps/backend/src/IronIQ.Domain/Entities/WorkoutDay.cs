namespace IronIQ.Domain.Entities;

public class WorkoutDay
{
    public Guid Id { get; private set; }
    public Guid WorkoutPlanId { get; private set; }
    public int DayOfWeek { get; private set; } // 0=Mon … 6=Sun
    public string? Name { get; private set; }
    public List<PlanExercise> Exercises { get; private set; } = [];

    private WorkoutDay() { }

    public static WorkoutDay Create(Guid planId, int dayOfWeek, string? name)
    {
        if (dayOfWeek < 0 || dayOfWeek > 6) throw new ArgumentException("DayOfWeek must be 0-6.");
        return new WorkoutDay
        {
            Id = Guid.NewGuid(),
            WorkoutPlanId = planId,
            DayOfWeek = dayOfWeek,
            Name = name?.Trim(),
            Exercises = []
        };
    }
}
