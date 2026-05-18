using IronIQ.Domain.Enums;

namespace IronIQ.Domain.Entities;

public class Achievement
{
    public Guid Id { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string IconEmoji { get; private set; } = string.Empty;
    public AchievementType Type { get; private set; }
    public int Threshold { get; private set; }
    public int DisplayOrder { get; private set; }

    private Achievement() { }

    public static Achievement Create(string key, string name, string description, string icon, AchievementType type, int threshold, int displayOrder)
        => new()
        {
            Id = Guid.NewGuid(),
            Key = key,
            Name = name,
            Description = description,
            IconEmoji = icon,
            Type = type,
            Threshold = threshold,
            DisplayOrder = displayOrder,
        };
}
