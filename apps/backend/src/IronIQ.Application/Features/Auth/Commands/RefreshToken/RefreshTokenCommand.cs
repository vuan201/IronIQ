using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Auth.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(Guid UserId, string RefreshToken) : IRequest<Result<AuthResponseDto>>;
