using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Subscriptions.Commands.ProcessRevenueCatWebhook;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IronIQ.API.Controllers;

[ApiController]
[Route("webhooks")]
public class WebhooksController(IMediator mediator) : ControllerBase
{
    [HttpPost("revenuecat")]
    public async Task<IActionResult> RevenueCat(CancellationToken ct)
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync(ct);
        var authHeader = Request.Headers.Authorization.FirstOrDefault() ?? string.Empty;

        var result = await mediator.Send(new ProcessRevenueCatWebhookCommand(payload, authHeader), ct);
        return result.IsSuccess
            ? Ok()
            : result.Error!.Type == ErrorType.Unauthorized ? Unauthorized() : BadRequest();
    }
}
