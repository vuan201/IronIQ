using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.DTOs;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand(
    string Name,
    string? Description,
    List<MuscleGroup> MuscleGroups,
    List<Equipment> Equipment,
    Difficulty Difficulty) : IRequest<Result<ExerciseDto>>;
