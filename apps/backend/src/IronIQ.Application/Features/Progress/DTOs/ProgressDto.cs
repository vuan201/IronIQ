namespace IronIQ.Application.Features.Progress.DTOs;

public record WeeklySessionCountDto(string WeekLabel, int Count);

public record VolumePointDto(string DateLabel, double TotalVolumeKg);

public record ProgressDto(
    IList<WeeklySessionCountDto> WeeklySessionCounts,
    IList<VolumePointDto> RecentVolume,
    int TotalSessionsAllTime,
    int TotalSetsAllTime);
