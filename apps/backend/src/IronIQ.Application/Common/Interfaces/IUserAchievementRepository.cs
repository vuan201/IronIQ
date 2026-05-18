using IronIQ.Domain.Entities;

namespace IronIQ.Application.Common.Interfaces;

public interface IUserAchievementRepository
{
    Task<IList<UserAchievement>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<UserAchievement> achievements, CancellationToken ct = default);
}
