using IronIQ.Application.Common.Models;
using MediatR;

namespace IronIQ.Application.Features.Coins.Queries.GetBalance;

public record GetBalanceQuery : IRequest<Result<CoinBalanceDto>>;
