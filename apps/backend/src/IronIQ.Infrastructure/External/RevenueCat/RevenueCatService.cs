using System.Text.Json;
using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace IronIQ.Infrastructure.External.RevenueCat;

public class RevenueCatService(IConfiguration configuration) : ISubscriptionService
{
    private static readonly HashSet<string> UpgradeEvents =
        ["INITIAL_PURCHASE", "RENEWAL", "UNCANCELLATION"];
    private static readonly HashSet<string> DowngradeEvents =
        ["CANCELLATION", "EXPIRATION", "BILLING_ISSUE"];

    public bool ValidateWebhookAuth(string authHeader)
    {
        var expected = configuration["RevenueCat:WebhookAuthHeader"];
        return !string.IsNullOrEmpty(expected) && authHeader == expected;
    }

    public SubscriptionWebhookEvent? ParseEvent(string payload)
    {
        try
        {
            using var doc = JsonDocument.Parse(payload);
            var evt = doc.RootElement.GetProperty("event");
            var type = evt.GetProperty("type").GetString() ?? string.Empty;
            var appUserId = evt.GetProperty("app_user_id").GetString() ?? string.Empty;

            if (!Guid.TryParse(appUserId, out var userId)) return null;

            if (UpgradeEvents.Contains(type))
                return new SubscriptionWebhookEvent(userId, SubscriptionTier.Premium);
            if (DowngradeEvents.Contains(type))
                return new SubscriptionWebhookEvent(userId, SubscriptionTier.Free);

            return null;
        }
        catch
        {
            return null;
        }
    }
}
