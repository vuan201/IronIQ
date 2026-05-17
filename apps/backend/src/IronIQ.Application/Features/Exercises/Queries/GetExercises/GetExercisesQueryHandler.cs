using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.Exercises.Queries.GetExercises;

public class GetExercisesQueryHandler(IExerciseRepository exerciseRepository)
    : IRequestHandler<GetExercisesQuery, Result<ExercisesPageDto>>
{
    public async Task<Result<ExercisesPageDto>> Handle(GetExercisesQuery query, CancellationToken ct)
    {
        var (items, total) = await exerciseRepository.GetAllAsync(
            query.Search, query.Muscle, query.Equipment, query.Difficulty,
            query.Page, query.PageSize, ct);

        var dtos = items.Select(MapToDto).ToList();
        return Result<ExercisesPageDto>.Success(new ExercisesPageDto(dtos, total, query.Page, query.PageSize));
    }

    private static ExerciseDto MapToDto(Exercise e) => new(
        e.Id,
        e.Name,
        e.Description,
        e.MuscleGroups.Select(m => m.ToString()).ToList(),
        e.Equipment.Select(eq => eq.ToString()).ToList(),
        e.Difficulty.ToString(),
        e.IsSystem,
        e.CreatedByUserId);
}
