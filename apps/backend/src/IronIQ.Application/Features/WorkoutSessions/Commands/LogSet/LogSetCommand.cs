using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.LogSet;

public record LogSetCommand(
    Guid SessionId,
    Guid ExerciseId,
    int SetNumber,
    float? WeightKg,
    int? Reps,
    int? DurationSeconds) : IRequest<Result<SetLogDto>>;
