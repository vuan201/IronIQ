using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Users.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Users.Queries.GetMyProfile;

public class GetMyProfileQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetMyProfileQuery, Result<UserProfileDto>>
{
    public async Task<Result<UserProfileDto>> Handle(GetMyProfileQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<UserProfileDto>.Failure(Error.Unauthorized());

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null)
            return Result<UserProfileDto>.Failure(Error.NotFound("User", currentUser.UserId));

        return Result<UserProfileDto>.Success(new UserProfileDto(
            user.Id,
            user.Email,
            user.Profile.Name,
            user.Profile.Age,
            user.Profile.HeightCm,
            user.Profile.WeightKg,
            user.Profile.Goal?.ToString(),
            user.Profile.Level?.ToString(),
            user.CoinBalance,
            user.SubscriptionTier.ToString(),
            user.CreatedAt,
            user.CurrentStreak,
            user.LongestStreak,
            user.IsLeaderboardOptIn));
    }
}
