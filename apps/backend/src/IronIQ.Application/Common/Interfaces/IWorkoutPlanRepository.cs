using IronIQ.Domain.Entities;

namespace IronIQ.Application.Common.Interfaces;

public interface IWorkoutPlanRepository
{
    Task<IList<WorkoutPlan>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<WorkoutPlan?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(WorkoutPlan plan, CancellationToken ct = default);
    Task DeleteAsync(WorkoutPlan plan, CancellationToken ct = default);
    Task<int> CountActiveByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<int> CountAIGeneratedThisMonthAsync(Guid userId, CancellationToken ct = default);
}
