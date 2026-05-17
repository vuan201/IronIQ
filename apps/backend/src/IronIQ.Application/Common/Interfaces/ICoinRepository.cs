using IronIQ.Domain.Entities;

namespace IronIQ.Application.Common.Interfaces;

public interface ICoinRepository
{
    Task<bool> ExternalTransactionExistsAsync(string externalTransactionId, CancellationToken ct = default);
    Task AddAsync(CoinTransaction transaction, CancellationToken ct = default);
}
