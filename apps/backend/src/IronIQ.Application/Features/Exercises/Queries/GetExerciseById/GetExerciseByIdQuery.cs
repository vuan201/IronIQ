using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQuery(Guid Id) : IRequest<Result<ExerciseDto>>;
