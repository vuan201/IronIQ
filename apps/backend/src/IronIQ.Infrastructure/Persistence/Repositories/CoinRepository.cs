using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IronIQ.Infrastructure.Persistence.Repositories;

public class CoinRepository(AppDbContext db) : ICoinRepository
{
    public Task<bool> ExternalTransactionExistsAsync(string externalTransactionId, CancellationToken ct = default)
        => db.CoinTransactions.AnyAsync(t => t.ExternalTransactionId == externalTransactionId, ct);

    public async Task AddAsync(CoinTransaction transaction, CancellationToken ct = default)
        => await db.CoinTransactions.AddAsync(transaction, ct);
}
