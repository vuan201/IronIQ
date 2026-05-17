using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Coins.Queries.GetBalance;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.Coins.Commands.SpendCoins;

public record SpendCoinsCommand(int Amount, CoinTransactionType Type, string Reason) : IRequest<Result<CoinBalanceDto>>;
