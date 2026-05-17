using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using MediatR;

namespace IronIQ.Application.Features.AiCoach.Queries.GetSessionReview;

public record GetSessionReviewQuery(Guid SessionId) : IRequest<Result<SessionReviewDto>>;
