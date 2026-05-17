using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using IronIQ.Application.Features.AiCoach.Queries.AskCoach;
using IronIQ.Application.Features.AiCoach.Queries.GetProgressionSuggestions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("ai-coach")]
[Authorize]
public class AiCoachController(IMediator mediator) : ControllerBase
{
    [HttpPost("ask")]
    public async Task<IActionResult> Ask(AskCoachRequest request, CancellationToken ct)
    {
        var query = new AskCoachQuery(request.Message, request.History);
        var result = await mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpGet("progression-suggestions/{sessionId:guid}")]
    public async Task<IActionResult> GetProgressionSuggestions(Guid sessionId, CancellationToken ct)
    {
        var query = new GetProgressionSuggestionsQuery(sessionId);
        var result = await mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    private IActionResult MapError(Error error) => error.Type switch
    {
        ErrorType.Unauthorized => Unauthorized(error.Message),
        _ => BadRequest(error.Message)
    };
}

public record AskCoachRequest(
    string Message,
    IList<CoachMessageDto> History);
