using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Auth.DTOs;
using IronIQ.Domain.Entities;
using MediatR;

namespace IronIQ.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RegisterCommand command, CancellationToken ct)
    {
        if (await userRepository.ExistsAsync(command.Email, ct))
            return Result<AuthResponseDto>.Failure(Error.Conflict("Email already registered."));

        var passwordHash = passwordHasher.Hash(command.Password);
        var user = User.Create(command.Email, passwordHash);

        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email);
        var (refreshToken, tokenHash, expiry) = jwtService.GenerateRefreshToken();
        user.SetRefreshToken(tokenHash, expiry);

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(accessToken, refreshToken, user.Id, user.Email));
    }
}
