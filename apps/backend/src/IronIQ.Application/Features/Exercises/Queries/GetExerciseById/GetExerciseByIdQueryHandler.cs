using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.Exercises.Queries.GetExerciseById;

public class GetExerciseByIdQueryHandler(IExerciseRepository exerciseRepository)
    : IRequestHandler<GetExerciseByIdQuery, Result<ExerciseDto>>
{
    public async Task<Result<ExerciseDto>> Handle(GetExerciseByIdQuery query, CancellationToken ct)
    {
        var exercise = await exerciseRepository.GetByIdAsync(query.Id, ct);
        if (exercise is null)
            return Result<ExerciseDto>.Failure(Error.NotFound("Exercise", query.Id));

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
