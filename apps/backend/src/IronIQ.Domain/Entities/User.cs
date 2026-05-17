using IronIQ.Domain.Enums;
using IronIQ.Domain.ValueObjects;

namespace IronIQ.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public int CoinBalance { get; private set; }
    public SubscriptionTier SubscriptionTier { get; private set; }
    public UserProfile Profile { get; private set; } = null!;
    public string? RefreshTokenHash { get; private set; }
    public DateTime? RefreshTokenExpiry { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email.ToLowerInvariant().Trim(),
            PasswordHash = passwordHash,
            CoinBalance = 0,
            SubscriptionTier = SubscriptionTier.Free,
            Profile = new UserProfile(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetRefreshToken(string tokenHash, DateTime expiry)
    {
        RefreshTokenHash = tokenHash;
        RefreshTokenExpiry = expiry;
    }

    public void UpdateProfile(string? name, int? age, float? heightCm, float? weightKg, FitnessGoal? goal, FitnessLevel? level)
    {
        Profile.Name = name;
        Profile.Age = age;
        Profile.HeightCm = heightCm;
        Profile.WeightKg = weightKg;
        Profile.Goal = goal;
        Profile.Level = level;
    }

    public bool IsRefreshTokenValid(string tokenHash)
        => RefreshTokenHash == tokenHash && RefreshTokenExpiry > DateTime.UtcNow;

    public void RevokeRefreshToken()
    {
        RefreshTokenHash = null;
        RefreshTokenExpiry = null;
    }

    public bool EarnCoins(int amount)
    {
        if (amount <= 0) return false;
        CoinBalance += amount;
        return true;
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || CoinBalance < amount) return false;
        CoinBalance -= amount;
        return true;
    }
}
