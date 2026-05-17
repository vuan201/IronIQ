using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Auth.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(LoginCommand command, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, ct);
        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
            return Result<AuthResponseDto>.Failure(Error.Unauthorized("Invalid credentials."));

        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email);
        var (refreshToken, tokenHash, expiry) = jwtService.GenerateRefreshToken();
        user.SetRefreshToken(tokenHash, expiry);

        await unitOfWork.SaveChangesAsync(ct);

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(accessToken, refreshToken, user.Id, user.Email));
    }
}
