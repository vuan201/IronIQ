using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using IronIQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Users.FindAsync([id], ct);

    public Task<bool> ExistsAsync(string email, CancellationToken ct = default)
        => db.Users.AnyAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
        => await db.Users.AddAsync(user, ct);

    public async Task<IList<User>> GetLeaderboardAsync(int limit, CancellationToken ct = default)
        => await db.Users
            .Where(u => u.IsLeaderboardOptIn)
            .OrderByDescending(u => u.CurrentStreak)
            .ThenByDescending(u => u.LongestStreak)
            .Take(limit)
            .ToListAsync(ct);
}
