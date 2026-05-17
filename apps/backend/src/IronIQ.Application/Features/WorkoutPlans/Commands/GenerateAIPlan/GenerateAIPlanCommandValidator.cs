using FluentValidation;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.GenerateAIPlan;

public class GenerateAIPlanCommandValidator : AbstractValidator<GenerateAIPlanCommand>
{
    public GenerateAIPlanCommandValidator()
    {
        RuleFor(x => x.Goal).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FitnessLevel).NotEmpty();
        RuleFor(x => x.DaysPerWeek).InclusiveBetween(1, 7);
    }
}
