using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.DTOs;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.Exercises.Queries.GetExercises;

public record GetExercisesQuery(
    string? Search = null,
    MuscleGroup? Muscle = null,
    Equipment? Equipment = null,
    Difficulty? Difficulty = null,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<ExercisesPageDto>>;
