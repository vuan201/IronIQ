using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.Commands.CompleteSession;
using IronIQ.Application.Features.WorkoutSessions.Commands.LogSet;
using IronIQ.Application.Features.WorkoutSessions.Commands.StartSession;
using IronIQ.Application.Features.WorkoutSessions.Queries.GetSessionHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("workout-sessions")]
[Authorize]
public class WorkoutSessionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetSessionHistoryQuery(page, pageSize), ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPost]
    public async Task<IActionResult> Start(StartSessionRequest request, CancellationToken ct)
    {
        var command = new StartSessionCommand(request.WorkoutPlanId, request.WorkoutDayId);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetHistory), result.Value) : MapError(result.Error!);
    }

    [HttpPost("{id:guid}/sets")]
    public async Task<IActionResult> LogSet(Guid id, LogSetRequest request, CancellationToken ct)
    {
        var command = new LogSetCommand(id, request.ExerciseId, request.SetNumber, request.WeightKg, request.Reps, request.DurationSeconds);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CompleteSessionRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CompleteSessionCommand(id, request.Notes), ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    private IActionResult MapError(Error error) => error.Type switch
    {
        ErrorType.NotFound => NotFound(error.Message),
        ErrorType.Unauthorized => Unauthorized(error.Message),
        _ => BadRequest(error.Message)
    };
}

public record StartSessionRequest(Guid? WorkoutPlanId, Guid? WorkoutDayId);
public record LogSetRequest(Guid ExerciseId, int SetNumber, float? WeightKg, int? Reps, int? DurationSeconds);
public record CompleteSessionRequest(string? Notes);
