using FluentValidation;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.CreateWorkoutPlan;

public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
        RuleFor(x => x.Days)
            .Must(d => d.Count <= 7).WithMessage("A plan can have at most 7 days.");
        RuleForEach(x => x.Days).ChildRules(day =>
        {
            day.RuleFor(d => d.DayOfWeek).InclusiveBetween(0, 6);
            day.RuleForEach(d => d.Exercises).ChildRules(ex =>
            {
                ex.RuleFor(e => e.Sets).GreaterThan(0);
                ex.RuleFor(e => e.RestSeconds).GreaterThanOrEqualTo(0);
            });
        });
    }
}
