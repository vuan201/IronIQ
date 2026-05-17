using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;

namespace IronIQ.Application.Common.Interfaces;

public interface IExerciseRepository
{
    Task<(IList<Exercise> Items, int Total)> GetAllAsync(
        string? search,
        MuscleGroup? muscle,
        Equipment? equipment,
        Difficulty? difficulty,
        int page,
        int pageSize,
        CancellationToken ct = default);

    Task<Exercise?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Exercise?> FindByNameAsync(string name, CancellationToken ct = default);
    Task<bool> ExistsSystemAsync(CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Exercise> exercises, CancellationToken ct = default);
    Task AddAsync(Exercise exercise, CancellationToken ct = default);
}
