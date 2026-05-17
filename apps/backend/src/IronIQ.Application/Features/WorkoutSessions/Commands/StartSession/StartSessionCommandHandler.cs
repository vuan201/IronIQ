using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.StartSession;

public class StartSessionCommandHandler(
    IWorkoutSessionRepository repository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<StartSessionCommand, Result<WorkoutSessionDto>>
{
    public async Task<Result<WorkoutSessionDto>> Handle(StartSessionCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<WorkoutSessionDto>.Failure(Error.Unauthorized());

        var session = WorkoutSession.Start(currentUser.UserId.Value, command.WorkoutPlanId, command.WorkoutDayId);

        await repository.AddAsync(session, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<WorkoutSessionDto>.Success(new WorkoutSessionDto(
            session.Id,
            session.WorkoutPlanId,
            session.WorkoutDayId,
            session.StartedAt,
            session.CompletedAt,
            session.Status.ToString(),
            session.Notes,
            []));
    }
}
