using IronIQ.Domain.Entities;

namespace IronIQ.Application.Common.Interfaces;

public interface IAchievementRepository
{
    Task<IList<Achievement>> GetAllAsync(CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
}
