namespace IronIQ.Domain.Entities;

public class ExerciseLog
{
    public Guid Id { get; private set; }
    public Guid WorkoutSessionId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public int Order { get; private set; }
    public List<SetLog> Sets { get; private set; } = [];

    public Exercise? Exercise { get; private set; }

    private ExerciseLog() { }

    public static ExerciseLog Create(Guid sessionId, Guid exerciseId, int order)
    {
        return new ExerciseLog
        {
            Id = Guid.NewGuid(),
            WorkoutSessionId = sessionId,
            ExerciseId = exerciseId,
            Order = order,
            Sets = []
        };
    }

    public void AddSet(SetLog set) => Sets.Add(set);
}
