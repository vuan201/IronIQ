using IronIQ.Domain.Entities;

namespace IronIQ.Application.Common.Interfaces;

public interface IWorkoutSessionRepository
{
    Task<WorkoutSession?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ExerciseLog?> GetExerciseLogAsync(Guid sessionId, Guid exerciseId, CancellationToken ct = default);
    Task<(IList<WorkoutSession> Items, int Total)> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
    Task<IList<WorkoutSession>> GetCompletedByUserSinceAsync(Guid userId, DateTime since, CancellationToken ct = default);
    Task AddAsync(WorkoutSession session, CancellationToken ct = default);
    Task AddExerciseLogAsync(ExerciseLog log, CancellationToken ct = default);
    Task AddSetLogAsync(SetLog set, CancellationToken ct = default);
}
