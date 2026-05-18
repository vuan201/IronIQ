using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Social.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Social.Queries.GetLeaderboard;

public record GetLeaderboardQuery(int Limit = 20) : IRequest<Result<IList<LeaderboardEntryDto>>>;
