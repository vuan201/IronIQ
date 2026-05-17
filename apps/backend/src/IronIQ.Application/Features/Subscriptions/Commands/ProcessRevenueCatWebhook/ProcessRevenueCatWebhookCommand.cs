using IronIQ.Application.Common.Models;
using MediatR;

namespace IronIQ.Application.Features.Subscriptions.Commands.ProcessRevenueCatWebhook;

public record ProcessRevenueCatWebhookCommand(string Payload, string AuthHeader) : IRequest<Result>;
