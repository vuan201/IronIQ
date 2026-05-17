using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using IronIQ.Application.Features.WorkoutSessions.Events;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.CompleteSession;

public class CompleteSessionCommandHandler(
    IWorkoutSessionRepository repository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork,
    IPublisher publisher)
    : IRequestHandler<CompleteSessionCommand, Result<WorkoutSessionDto>>
{
    public async Task<Result<WorkoutSessionDto>> Handle(CompleteSessionCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<WorkoutSessionDto>.Failure(Error.Unauthorized());

        var session = await repository.GetByIdAsync(command.SessionId, ct);
        if (session is null)
            return Result<WorkoutSessionDto>.Failure(Error.NotFound("WorkoutSession", command.SessionId));

        if (session.UserId != currentUser.UserId.Value)
            return Result<WorkoutSessionDto>.Failure(Error.Unauthorized());

        if (session.Status != SessionStatus.Active)
            return Result<WorkoutSessionDto>.Failure(Error.Failure("Session is not active."));

        session.Complete(command.Notes);
        await unitOfWork.SaveChangesAsync(ct);

        await publisher.Publish(new WorkoutSessionCompletedEvent(session.Id, session.UserId), ct);

        return Result<WorkoutSessionDto>.Success(new WorkoutSessionDto(
            session.Id,
            session.WorkoutPlanId,
            session.WorkoutDayId,
            session.StartedAt,
            session.CompletedAt,
            session.Status.ToString(),
            session.Notes,
            session.ExerciseLogs.Select(l => new ExerciseLogDto(
                l.Id,
                l.ExerciseId,
                l.Exercise?.Name ?? string.Empty,
                l.Order,
                l.Sets.Select(s => new SetLogDto(s.Id, s.SetNumber, s.WeightKg, s.Reps, s.DurationSeconds, s.IsCompleted)).ToList()
            )).ToList()));
    }
}
