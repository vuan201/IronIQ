using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Coins.Queries.GetBalance;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.Coins.Commands.SpendCoins;

public class SpendCoinsCommandHandler(
    IUserRepository userRepository,
    ICoinRepository coinRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SpendCoinsCommand, Result<CoinBalanceDto>>
{
    public async Task<Result<CoinBalanceDto>> Handle(SpendCoinsCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<CoinBalanceDto>.Failure(Error.Unauthorized());

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null)
            return Result<CoinBalanceDto>.Failure(Error.NotFound("User", currentUser.UserId.Value));

        if (!user.SpendCoins(command.Amount))
            return Result<CoinBalanceDto>.Failure(Error.Failure("Insufficient coin balance."));

        var transaction = CoinTransaction.CreateSpend(
            user.Id,
            command.Amount,
            command.Type,
            command.Reason);

        await coinRepository.AddAsync(transaction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<CoinBalanceDto>.Success(new CoinBalanceDto(user.CoinBalance));
    }
}
