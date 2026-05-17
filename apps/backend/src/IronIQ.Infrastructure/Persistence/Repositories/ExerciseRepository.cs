using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class ExerciseRepository(AppDbContext db) : IExerciseRepository
{
    public async Task<(IList<Exercise> Items, int Total)> GetAllAsync(
        string? search,
        MuscleGroup? muscle,
        Equipment? equipment,
        Difficulty? difficulty,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = db.Exercises.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.Contains(search));

        if (muscle.HasValue)
            query = query.Where(e => e.MuscleGroups.Contains(muscle.Value));

        if (equipment.HasValue)
            query = query.Where(e => e.Equipment.Contains(equipment.Value));

        if (difficulty.HasValue)
            query = query.Where(e => e.Difficulty == difficulty.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<Exercise?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Exercises.FindAsync([id], ct);

    public Task<bool> ExistsSystemAsync(CancellationToken ct = default)
        => db.Exercises.AnyAsync(e => e.IsSystem, ct);

    public async Task AddRangeAsync(IEnumerable<Exercise> exercises, CancellationToken ct = default)
        => await db.Exercises.AddRangeAsync(exercises, ct);

    public async Task AddAsync(Exercise exercise, CancellationToken ct = default)
        => await db.Exercises.AddAsync(exercise, ct);
}
