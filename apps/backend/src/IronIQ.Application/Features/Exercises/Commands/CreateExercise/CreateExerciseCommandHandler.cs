using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.Exercises.Commands.CreateExercise;

public class CreateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateExerciseCommand, Result<ExerciseDto>>
{
    public async Task<Result<ExerciseDto>> Handle(CreateExerciseCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<ExerciseDto>.Failure(Error.Unauthorized());

        var exercise = Exercise.CreateUserDefined(
            currentUser.UserId.Value,
            command.Name,
            command.Description,
            command.MuscleGroups,
            command.Equipment,
            command.Difficulty);

        await exerciseRepository.AddAsync(exercise, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<ExerciseDto>.Success(new ExerciseDto(
            exercise.Id,
            exercise.Name,
            exercise.Description,
            exercise.MuscleGroups.Select(m => m.ToString()).ToList(),
            exercise.Equipment.Select(eq => eq.ToString()).ToList(),
            exercise.Difficulty.ToString(),
            exercise.IsSystem,
            exercise.CreatedByUserId));
    }
}
