using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.Commands.CreateWorkoutPlan;
using IronIQ.Application.Features.WorkoutPlans.Commands.DeleteWorkoutPlan;
using IronIQ.Application.Features.WorkoutPlans.Commands.GenerateAIPlan;
using IronIQ.Application.Features.WorkoutPlans.Commands.UpdateWorkoutPlan;
using IronIQ.Application.Features.WorkoutPlans.Queries.GetMyPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("workout-plans")]
[Authorize]
public class WorkoutPlansController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMyPlans(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyPlansQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkoutPlanRequest request, CancellationToken ct)
    {
        var command = new CreateWorkoutPlanCommand(request.Name, request.Description, request.Days);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetMyPlans), result.Value) : MapError(result.Error!);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateWorkoutPlanRequest request, CancellationToken ct)
    {
        var command = new UpdateWorkoutPlanCommand(id, request.Name, request.Description, request.Days);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(GenerateAIPlanRequest request, CancellationToken ct)
    {
        var command = new GenerateAIPlanCommand(
            request.Goal,
            request.FitnessLevel,
            request.DaysPerWeek,
            request.AvailableEquipment,
            request.FocusAreas);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetMyPlans), result.Value) : MapError(result.Error!);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteWorkoutPlanCommand(id), ct);
        return result.IsSuccess ? NoContent() : MapError(result.Error!);
    }

    private IActionResult MapError(Error error) => error.Type switch
    {
        ErrorType.NotFound => NotFound(error.Message),
        ErrorType.Unauthorized => Unauthorized(error.Message),
        ErrorType.Conflict => Conflict(error.Message),
        _ => BadRequest(error.Message)
    };
}

public record GenerateAIPlanRequest(
    string Goal,
    string FitnessLevel,
    int DaysPerWeek,
    List<string> AvailableEquipment,
    List<string> FocusAreas);

public record CreateWorkoutPlanRequest(
    string Name,
    string? Description,
    List<CreateWorkoutDayDto> Days);

public record UpdateWorkoutPlanRequest(
    string Name,
    string? Description,
    List<CreateWorkoutDayDto> Days);
