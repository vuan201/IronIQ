using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class WorkoutSessionRepository(AppDbContext db) : IWorkoutSessionRepository
{
    public async Task<WorkoutSession?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.WorkoutSessions
            .Include(s => s.ExerciseLogs)
                .ThenInclude(l => l.Sets)
            .Include(s => s.ExerciseLogs)
                .ThenInclude(l => l.Exercise)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<ExerciseLog?> GetExerciseLogAsync(Guid sessionId, Guid exerciseId, CancellationToken ct = default)
        => await db.ExerciseLogs
            .Include(l => l.Sets)
            .FirstOrDefaultAsync(l => l.WorkoutSessionId == sessionId && l.ExerciseId == exerciseId, ct);

    public async Task<(IList<WorkoutSession> Items, int Total)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var query = db.WorkoutSessions
            .Include(s => s.ExerciseLogs)
                .ThenInclude(l => l.Sets)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartedAt);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task AddAsync(WorkoutSession session, CancellationToken ct = default)
        => await db.WorkoutSessions.AddAsync(session, ct);

    public async Task AddExerciseLogAsync(ExerciseLog log, CancellationToken ct = default)
        => await db.ExerciseLogs.AddAsync(log, ct);

    public async Task AddSetLogAsync(SetLog set, CancellationToken ct = default)
        => await db.SetLogs.AddAsync(set, ct);
}
