using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Progress.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Progress.Queries.GetProgress;

public record GetProgressQuery(int WeeksBack = 8) : IRequest<Result<ProgressDto>>;
