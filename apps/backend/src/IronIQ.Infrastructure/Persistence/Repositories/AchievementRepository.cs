using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class AchievementRepository(AppDbContext db) : IAchievementRepository
{
    public async Task<IList<Achievement>> GetAllAsync(CancellationToken ct = default)
        => await db.Achievements.OrderBy(a => a.DisplayOrder).ToListAsync(ct);

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await db.Achievements.AnyAsync(ct);
}
