using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Exercises.Commands.CreateExercise;
using IronIQ.Application.Features.Exercises.Queries.GetExerciseById;
using IronIQ.Application.Features.Exercises.Queries.GetExercises;
using IronIQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("exercises")]
[Authorize]
public class ExercisesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? muscle,
        [FromQuery] string? equipment,
        [FromQuery] string? difficulty,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        Enum.TryParse<MuscleGroup>(muscle, out var muscleGroup);
        Enum.TryParse<Equipment>(equipment, out var equipmentValue);
        Enum.TryParse<Difficulty>(difficulty, out var difficultyValue);

        var query = new GetExercisesQuery(
            search,
            muscle is not null ? muscleGroup : null,
            equipment is not null ? equipmentValue : null,
            difficulty is not null ? difficultyValue : null,
            page, pageSize);

        var result = await mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error!.Message);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetExerciseByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error!.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExerciseRequest request, CancellationToken ct)
    {
        var command = new CreateExerciseCommand(
            request.Name,
            request.Description,
            request.MuscleGroups,
            request.Equipment,
            request.Difficulty);

        var result = await mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : MapError(result.Error!);
    }

    private IActionResult MapError(Error error) => error.Type switch
    {
        ErrorType.Unauthorized => Unauthorized(error.Message),
        _ => BadRequest(error.Message)
    };
}

public record CreateExerciseRequest(
    string Name,
    string? Description,
    List<MuscleGroup> MuscleGroups,
    List<Equipment> Equipment,
    Difficulty Difficulty);
