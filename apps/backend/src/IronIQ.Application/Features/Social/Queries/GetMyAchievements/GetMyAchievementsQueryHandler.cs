using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Social.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Social.Queries.GetMyAchievements;

public class GetMyAchievementsQueryHandler(
    IAchievementRepository achievementRepository,
    IUserAchievementRepository userAchievementRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetMyAchievementsQuery, Result<IList<AchievementDto>>>
{
    public async Task<Result<IList<AchievementDto>>> Handle(GetMyAchievementsQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<IList<AchievementDto>>.Failure(Error.Unauthorized());

        var all = await achievementRepository.GetAllAsync(ct);
        var unlocked = await userAchievementRepository.GetByUserIdAsync(currentUser.UserId.Value, ct);
        var unlockedMap = unlocked.ToDictionary(u => u.AchievementId);

        var result = all.Select(a =>
        {
            var isUnlocked = unlockedMap.TryGetValue(a.Id, out var ua);
            return new AchievementDto(
                a.Id, a.Key, a.Name, a.Description, a.IconEmoji,
                a.Type.ToString(), a.Threshold, a.DisplayOrder,
                isUnlocked, isUnlocked ? ua!.UnlockedAt : null);
        }).ToList();

        return Result<IList<AchievementDto>>.Success(result);
    }
}
