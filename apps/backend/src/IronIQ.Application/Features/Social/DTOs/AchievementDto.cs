namespace IronIQ.Application.Features.Social.DTOs;

public record AchievementDto(
    Guid Id,
    string Key,
    string Name,
    string Description,
    string IconEmoji,
    string Type,
    int Threshold,
    int DisplayOrder,
    bool IsUnlocked,
    DateTime? UnlockedAt);
