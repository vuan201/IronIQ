using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutSessions.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Commands.StartSession;

public record StartSessionCommand(
    Guid? WorkoutPlanId,
    Guid? WorkoutDayId) : IRequest<Result<WorkoutSessionDto>>;
