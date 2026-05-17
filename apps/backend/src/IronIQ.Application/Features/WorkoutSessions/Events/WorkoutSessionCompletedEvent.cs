using MediatR;

namespace IronIQ.Application.Features.WorkoutSessions.Events;

public record WorkoutSessionCompletedEvent(Guid SessionId, Guid UserId) : INotification;
