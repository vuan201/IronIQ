using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Progress.Queries.GetProgress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("progress")]
[Authorize]
public class ProgressController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int weeksBack = 8, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProgressQuery(weeksBack), ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error!.Message);
    }
}
