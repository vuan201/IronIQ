using IronIQ.Domain.Enums;

namespace IronIQ.Domain.ValueObjects;

public class UserProfile
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public float? HeightCm { get; set; }
    public float? WeightKg { get; set; }
    public FitnessGoal? Goal { get; set; }
    public FitnessLevel? Level { get; set; }
}
