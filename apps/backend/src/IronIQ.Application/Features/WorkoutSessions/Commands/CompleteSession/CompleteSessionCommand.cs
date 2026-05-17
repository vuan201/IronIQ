using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.CompleteSession;

public record CompleteSessionCommand(Guid SessionId, string? Notes) : IRequest<Result<WorkoutSessionDto>>;
