using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Coins.Queries.GetBalance;
using MediatR;

namespace IronIQ.Application.Features.Coins.Commands.EarnFromAd;

public record EarnFromAdCommand(string ExternalTransactionId) : IRequest<Result<CoinBalanceDto>>;
