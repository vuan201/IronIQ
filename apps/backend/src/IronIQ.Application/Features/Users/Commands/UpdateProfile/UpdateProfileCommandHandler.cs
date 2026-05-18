using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Users.DTOs;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProfileCommand, Result<UserProfileDto>>
{
    public async Task<Result<UserProfileDto>> Handle(UpdateProfileCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<UserProfileDto>.Failure(Error.Unauthorized());

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null)
            return Result<UserProfileDto>.Failure(Error.NotFound("User", currentUser.UserId));

        Enum.TryParse<FitnessGoal>(command.Goal, out var goal);
        Enum.TryParse<FitnessLevel>(command.Level, out var level);

        user.UpdateProfile(
            command.Name,
            command.Age,
            command.HeightCm,
            command.WeightKg,
            command.Goal is not null ? goal : null,
            command.Level is not null ? level : null);

        if (command.IsLeaderboardOptIn.HasValue)
            user.SetLeaderboardOptIn(command.IsLeaderboardOptIn.Value);

        await unitOfWork.SaveChangesAsync(ct);

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
