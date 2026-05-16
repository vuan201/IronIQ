using IronIQ.Application.Common.Interfaces;
using MediatR;

namespace IronIQ.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        // Only wrap commands (not queries) in a transaction
        if (request.GetType().Name.EndsWith("Query"))
            return await next();

        await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var response = await next();
            await unitOfWork.CommitTransactionAsync(ct);
            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}
