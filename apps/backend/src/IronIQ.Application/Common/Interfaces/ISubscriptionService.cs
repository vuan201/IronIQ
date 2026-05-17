using IronIQ.Domain.Enums;

namespace IronIQ.Application.Common.Interfaces;

public interface ISubscriptionService
{
    bool ValidateWebhookAuth(string authHeader);
    SubscriptionWebhookEvent? ParseEvent(string payload);
}

public record SubscriptionWebhookEvent(Guid UserId, SubscriptionTier NewTier);
