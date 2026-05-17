using IronIQ.Domain.Enums;

namespace IronIQ.Domain.Entities;

public class Exercise
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public List<MuscleGroup> MuscleGroups { get; private set; } = [];
    public List<Equipment> Equipment { get; private set; } = [];
    public Difficulty Difficulty { get; private set; }
    public bool IsSystem { get; private set; }
    public Guid? CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Exercise() { }

    public static Exercise CreateSystem(
        string name,
        string? description,
        List<MuscleGroup> muscleGroups,
        List<Equipment> equipment,
        Difficulty difficulty)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Exercise name is required.");
        return new Exercise
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            MuscleGroups = muscleGroups,
            Equipment = equipment,
            Difficulty = difficulty,
            IsSystem = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Exercise CreateUserDefined(
        Guid userId,
        string name,
        string? description,
        List<MuscleGroup> muscleGroups,
        List<Equipment> equipment,
        Difficulty difficulty)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Exercise name is required.");
        return new Exercise
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            MuscleGroups = muscleGroups,
            Equipment = equipment,
            Difficulty = difficulty,
            IsSystem = false,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
