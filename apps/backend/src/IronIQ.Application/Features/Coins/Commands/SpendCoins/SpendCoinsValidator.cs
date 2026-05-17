using FluentValidation;

namespace IronIQ.Application.Features.Coins.Commands.SpendCoins;

public class SpendCoinsValidator : AbstractValidator<SpendCoinsCommand>
{
    public SpendCoinsValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(200);
    }
}
