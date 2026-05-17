using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Auth.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
