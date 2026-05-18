namespace IronIQ.Application.Features.Users.DTOs;

public record UserProfileDto(
    Guid Id,
    string Email,
    string? Name,
    int? Age,
    float? HeightCm,
    float? WeightKg,
    string? Goal,
    string? Level,
    int CoinBalance,
    string SubscriptionTier,
    DateTime CreatedAt,
    int CurrentStreak,
    int LongestStreak);
