using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class UserAchievementRepository(AppDbContext db) : IUserAchievementRepository
{
    public async Task<IList<UserAchievement>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await db.UserAchievements.Where(ua => ua.UserId == userId).ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<UserAchievement> achievements, CancellationToken ct = default)
        => await db.UserAchievements.AddRangeAsync(achievements, ct);
}
