using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.LogSet;

public class LogSetCommandHandler(
    IWorkoutSessionRepository repository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LogSetCommand, Result<SetLogDto>>
{
    public async Task<Result<SetLogDto>> Handle(LogSetCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<SetLogDto>.Failure(Error.Unauthorized());

        var session = await repository.GetByIdAsync(command.SessionId, ct);
        if (session is null)
            return Result<SetLogDto>.Failure(Error.NotFound("WorkoutSession", command.SessionId));

        if (session.UserId != currentUser.UserId.Value)
            return Result<SetLogDto>.Failure(Error.Unauthorized());

        if (session.Status != SessionStatus.Active)
            return Result<SetLogDto>.Failure(Error.Failure("Session is not active."));

        var exerciseLog = await repository.GetExerciseLogAsync(command.SessionId, command.ExerciseId, ct);
        if (exerciseLog is null)
        {
            var order = session.ExerciseLogs.Count + 1;
            exerciseLog = ExerciseLog.Create(session.Id, command.ExerciseId, order);
            await repository.AddExerciseLogAsync(exerciseLog, ct);
        }

        var set = SetLog.Create(exerciseLog.Id, command.SetNumber, command.WeightKg, command.Reps, command.DurationSeconds);
        await repository.AddSetLogAsync(set, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<SetLogDto>.Success(new SetLogDto(
            set.Id,
            set.SetNumber,
            set.WeightKg,
            set.Reps,
            set.DurationSeconds,
            set.IsCompleted));
    }
}
