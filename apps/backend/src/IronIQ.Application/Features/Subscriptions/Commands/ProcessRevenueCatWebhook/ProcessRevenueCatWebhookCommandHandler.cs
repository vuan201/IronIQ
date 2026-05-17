using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using MediatR;

namespace IronIQ.Application.Features.Subscriptions.Commands.ProcessRevenueCatWebhook;

public class ProcessRevenueCatWebhookCommandHandler(
    ISubscriptionService subscriptionService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ProcessRevenueCatWebhookCommand, Result>
{
    public async Task<Result> Handle(ProcessRevenueCatWebhookCommand command, CancellationToken ct)
    {
        if (!subscriptionService.ValidateWebhookAuth(command.AuthHeader))
            return Result.Failure(Error.Unauthorized());

        var evt = subscriptionService.ParseEvent(command.Payload);
        if (evt is null) return Result.Success();

        var user = await userRepository.GetByIdAsync(evt.UserId, ct);
        if (user is null) return Result.Success();

        user.UpdateSubscriptionTier(evt.NewTier);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
