using IronIQ.Domain.Entities;

namespace IronIQ.Application.Common.Interfaces;

public interface IProgressionSuggestionRepository
{
    Task AddRangeAsync(IEnumerable<ProgressionSuggestion> suggestions, CancellationToken ct = default);
    Task<IList<ProgressionSuggestion>> GetBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
}
