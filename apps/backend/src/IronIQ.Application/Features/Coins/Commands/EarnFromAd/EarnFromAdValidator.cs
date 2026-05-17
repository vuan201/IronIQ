using FluentValidation;

namespace IronIQ.Application.Features.Coins.Commands.EarnFromAd;

public class EarnFromAdValidator : AbstractValidator<EarnFromAdCommand>
{
    public EarnFromAdValidator()
    {
        RuleFor(x => x.ExternalTransactionId)
            .NotEmpty()
            .MaximumLength(200);
    }
}
