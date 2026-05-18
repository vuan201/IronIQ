using IronIQ.Application.Features.Social.Queries.GetLeaderboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("leaderboard")]
[Authorize]
public class LeaderboardController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetLeaderboardQuery(Math.Clamp(limit, 1, 50)), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error!.Message);
    }
}
