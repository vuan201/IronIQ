using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Users.Commands.UpdateProfile;
using IronIQ.Application.Features.Users.Queries.GetMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyProfileQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request, CancellationToken ct)
    {
        var command = new UpdateProfileCommand(
            request.Name,
            request.Age,
            request.HeightCm,
            request.WeightKg,
            request.Goal,
            request.Level,
            request.IsLeaderboardOptIn);

        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    private IActionResult MapError(Error error) => error.Type switch
    {
        ErrorType.NotFound => NotFound(error.Message),
        ErrorType.Unauthorized => Unauthorized(error.Message),
        _ => BadRequest(error.Message)
    };
}

public record UpdateProfileRequest(
    string? Name,
    int? Age,
    float? HeightCm,
    float? WeightKg,
    string? Goal,
    string? Level,
    bool? IsLeaderboardOptIn);
