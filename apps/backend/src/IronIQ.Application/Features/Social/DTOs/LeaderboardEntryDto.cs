namespace IronIQ.Application.Features.Social.DTOs;

public record LeaderboardEntryDto(
    int Rank,
    string DisplayName,
    int CurrentStreak,
    int LongestStreak);
