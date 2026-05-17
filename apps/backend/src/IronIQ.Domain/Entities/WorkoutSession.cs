using IronIQ.Domain.Enums;

namespace IronIQ.Domain.Entities;

public class WorkoutSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? WorkoutPlanId { get; private set; }
    public Guid? WorkoutDayId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public SessionStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public List<ExerciseLog> ExerciseLogs { get; private set; } = [];

    private WorkoutSession() { }

    public static WorkoutSession Start(Guid userId, Guid? workoutPlanId, Guid? workoutDayId)
    {
        return new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            WorkoutPlanId = workoutPlanId,
            WorkoutDayId = workoutDayId,
            StartedAt = DateTime.UtcNow,
            Status = SessionStatus.Active,
            ExerciseLogs = []
        };
    }

    public void Complete(string? notes)
    {
        Status = SessionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        Notes = notes?.Trim();
    }

    public void Abandon()
    {
        Status = SessionStatus.Abandoned;
        CompletedAt = DateTime.UtcNow;
    }
}
