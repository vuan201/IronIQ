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
}
