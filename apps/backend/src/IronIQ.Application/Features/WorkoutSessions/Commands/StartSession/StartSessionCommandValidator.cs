using FluentValidation;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.StartSession;

public class StartSessionCommandValidator : AbstractValidator<StartSessionCommand>
{
    public StartSessionCommandValidator()
    {
        RuleFor(x => x.WorkoutDayId)
            .NotEmpty()
            .When(x => x.WorkoutPlanId.HasValue)
            .WithMessage("WorkoutDayId is required when WorkoutPlanId is provided.");
    }
}
