using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Social.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Social.Queries.GetLeaderboard;

public class GetLeaderboardQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetLeaderboardQuery, Result<IList<LeaderboardEntryDto>>>
{
    public async Task<Result<IList<LeaderboardEntryDto>>> Handle(GetLeaderboardQuery query, CancellationToken ct)
    {
        var users = await userRepository.GetLeaderboardAsync(query.Limit, ct);

        var entries = users
            .Select((u, index) => new LeaderboardEntryDto(
                Rank: index + 1,
                DisplayName: u.Profile.Name ?? "Anonymous",
                CurrentStreak: u.CurrentStreak,
                LongestStreak: u.LongestStreak))
            .ToList();

        return Result<IList<LeaderboardEntryDto>>.Success(entries);
    }
}
