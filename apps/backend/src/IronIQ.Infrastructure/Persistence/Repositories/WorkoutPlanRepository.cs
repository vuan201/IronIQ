using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class WorkoutPlanRepository(AppDbContext db) : IWorkoutPlanRepository
{
    public async Task<IList<WorkoutPlan>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await db.WorkoutPlans
            .Include(p => p.Days)
                .ThenInclude(d => d.Exercises)
                    .ThenInclude(e => e.Exercise)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync(ct);

    public async Task<WorkoutPlan?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.WorkoutPlans
            .Include(p => p.Days)
                .ThenInclude(d => d.Exercises)
                    .ThenInclude(e => e.Exercise)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(WorkoutPlan plan, CancellationToken ct = default)
        => await db.WorkoutPlans.AddAsync(plan, ct);

    public Task DeleteAsync(WorkoutPlan plan, CancellationToken ct = default)
    {
        db.WorkoutPlans.Remove(plan);
        return Task.CompletedTask;
    }

    public async Task<int> CountActiveByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await db.WorkoutPlans.CountAsync(p => p.UserId == userId && p.IsActive, ct);

    public async Task<int> CountAIGeneratedThisMonthAsync(Guid userId, CancellationToken ct = default)
    {
        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        return await db.WorkoutPlans
            .CountAsync(p => p.UserId == userId && p.IsAIGenerated && p.CreatedAt >= startOfMonth, ct);
    }
}
