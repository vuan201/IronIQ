using MediatR;
using Microsoft.Extensions.Logging;

namespace IronIQ.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName}", name);
        var start = DateTime.UtcNow;

        var response = await next();

        logger.LogInformation("Handled {RequestName} in {ElapsedMs}ms", name, (DateTime.UtcNow - start).TotalMilliseconds);
        return response;
    }
}
