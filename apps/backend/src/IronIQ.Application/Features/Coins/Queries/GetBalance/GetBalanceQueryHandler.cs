using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using MediatR;

namespace IronIQ.Application.Features.Coins.Queries.GetBalance;

public class GetBalanceQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetBalanceQuery, Result<CoinBalanceDto>>
{
    public async Task<Result<CoinBalanceDto>> Handle(GetBalanceQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<CoinBalanceDto>.Failure(Error.Unauthorized());

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null)
            return Result<CoinBalanceDto>.Failure(Error.NotFound("User", currentUser.UserId.Value));

        return Result<CoinBalanceDto>.Success(new CoinBalanceDto(user.CoinBalance));
    }
}
