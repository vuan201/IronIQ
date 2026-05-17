using FluentValidation;
using IronIQ.Domain.Enums;

namespace IronIQ.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100).When(x => x.Name is not null);
        RuleFor(x => x.Age).InclusiveBetween(10, 100).When(x => x.Age.HasValue);
        RuleFor(x => x.HeightCm).InclusiveBetween(100f, 250f).When(x => x.HeightCm.HasValue);
        RuleFor(x => x.WeightKg).InclusiveBetween(30f, 300f).When(x => x.WeightKg.HasValue);
        RuleFor(x => x.Goal)
            .Must(g => Enum.TryParse<FitnessGoal>(g, out _))
            .When(x => x.Goal is not null)
            .WithMessage("Invalid fitness goal.");
        RuleFor(x => x.Level)
            .Must(l => Enum.TryParse<FitnessLevel>(l, out _))
            .When(x => x.Level is not null)
            .WithMessage("Invalid fitness level.");
    }
}
