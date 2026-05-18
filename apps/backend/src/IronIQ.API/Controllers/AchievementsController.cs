using IronIQ.Application.Features.Social.Queries.GetMyAchievements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("achievements")]
[Authorize]
public class AchievementsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMy(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyAchievementsQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error!.Message);
    }
}
