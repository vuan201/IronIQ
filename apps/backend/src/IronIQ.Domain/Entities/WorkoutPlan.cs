namespace IronIQ.Domain.Entities;

public class WorkoutPlan
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsAIGenerated { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public List<WorkoutDay> Days { get; private set; } = [];

    private WorkoutPlan() { }

    public static WorkoutPlan Create(Guid userId, string name, string? description, bool isAIGenerated = false)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Plan name is required.");
        return new WorkoutPlan
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name.Trim(),
            Description = description?.Trim(),
            IsActive = true,
            IsAIGenerated = isAIGenerated,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Days = []
        };
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Plan name is required.");
        Name = name.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReplaceDays(List<WorkoutDay> days)
    {
        Days = days;
        UpdatedAt = DateTime.UtcNow;
    }
}
