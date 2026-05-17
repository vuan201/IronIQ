using IronIQ.Domain.Enums;

namespace IronIQ.Domain.Entities;

public class CoinTransaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public int Amount { get; private set; }
    public CoinTransactionType Type { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string? ExternalTransactionId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private CoinTransaction() { }

    public static CoinTransaction CreateEarn(
        Guid userId,
        int amount,
        CoinTransactionType type,
        string reason,
        string? externalTransactionId = null)
    {
        return new CoinTransaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = amount,
            Type = type,
            Reason = reason,
            ExternalTransactionId = externalTransactionId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CoinTransaction CreateSpend(
        Guid userId,
        int amount,
        CoinTransactionType type,
        string reason)
    {
        return new CoinTransaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = -amount,
            Type = type,
            Reason = reason,
            CreatedAt = DateTime.UtcNow
        };
    }
}
