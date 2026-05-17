using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Coins.Queries.GetBalance;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.Coins.Commands.EarnFromAd;

public class EarnFromAdCommandHandler(
    IUserRepository userRepository,
    ICoinRepository coinRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<EarnFromAdCommand, Result<CoinBalanceDto>>
{
    private const int CoinsPerAd = 10;

    public async Task<Result<CoinBalanceDto>> Handle(EarnFromAdCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<CoinBalanceDto>.Failure(Error.Unauthorized());

        if (await coinRepository.ExternalTransactionExistsAsync(command.ExternalTransactionId, ct))
            return Result<CoinBalanceDto>.Failure(Error.Conflict("Ad reward already claimed for this transaction."));

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null)
            return Result<CoinBalanceDto>.Failure(Error.NotFound("User", currentUser.UserId.Value));

        user.EarnCoins(CoinsPerAd);

        var transaction = CoinTransaction.CreateEarn(
            user.Id,
            CoinsPerAd,
            CoinTransactionType.EarnedFromAd,
            "Watched rewarded ad",
            command.ExternalTransactionId);

        await coinRepository.AddAsync(transaction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<CoinBalanceDto>.Success(new CoinBalanceDto(user.CoinBalance));
    }
}
