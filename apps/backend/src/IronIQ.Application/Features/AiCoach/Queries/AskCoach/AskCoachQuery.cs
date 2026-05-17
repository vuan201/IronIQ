using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using MediatR;

namespace IronIQ.Application.Features.AiCoach.Queries.AskCoach;

public record AskCoachQuery(
    string Message,
    IList<CoachMessageDto> History) : IRequest<Result<CoachResponseDto>>;
