using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Queries.GetSessionHistory;

public record GetSessionHistoryQuery(int Page = 1, int PageSize = 20) : IRequest<Result<SessionHistoryPageDto>>;
