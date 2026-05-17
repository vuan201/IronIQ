using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Coins.Commands.EarnFromAd;
using IronIQ.Application.Features.Coins.Commands.SpendCoins;
using IronIQ.Application.Features.Coins.Queries.GetBalance;
using IronIQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("coins")]
[Authorize]
public class CoinsController(IMediator mediator) : ControllerBase
{
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(CancellationToken ct)
    {
        var result = await mediator.Send(new GetBalanceQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPost("earn-from-ad")]
    public async Task<IActionResult> EarnFromAd(EarnFromAdRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new EarnFromAdCommand(request.ExternalTransactionId), ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    [HttpPost("spend")]
    public async Task<IActionResult> Spend(SpendCoinsRequest request, CancellationToken ct)
    {
        var command = new SpendCoinsCommand(request.Amount, request.Type, request.Reason);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error!);
    }

    private IActionResult MapError(Error error) => error.Type switch
    {
        ErrorType.NotFound => NotFound(error.Message),
        ErrorType.Conflict => Conflict(error.Message),
        ErrorType.Unauthorized => Unauthorized(error.Message),
        _ => BadRequest(error.Message)
    };
}

public record EarnFromAdRequest(string ExternalTransactionId);
public record SpendCoinsRequest(int Amount, CoinTransactionType Type, string Reason);
