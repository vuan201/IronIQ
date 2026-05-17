using FluentValidation;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.UpdateWorkoutPlan;

public class UpdateWorkoutPlanCommandValidator : AbstractValidator<UpdateWorkoutPlanCommand>
{
    public UpdateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.PlanId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
        RuleFor(x => x.Days).Must(d => d.Count <= 7).WithMessage("A plan can have at most 7 days.");
    }
}
