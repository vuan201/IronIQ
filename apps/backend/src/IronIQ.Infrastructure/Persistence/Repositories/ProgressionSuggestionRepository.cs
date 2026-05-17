using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class ProgressionSuggestionRepository(AppDbContext db) : IProgressionSuggestionRepository
{
    public async Task AddRangeAsync(IEnumerable<ProgressionSuggestion> suggestions, CancellationToken ct = default)
        => await db.ProgressionSuggestions.AddRangeAsync(suggestions, ct);

    public async Task<IList<ProgressionSuggestion>> GetBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
        => await db.ProgressionSuggestions
            .Where(s => s.SessionId == sessionId)
            .ToListAsync(ct);
}
