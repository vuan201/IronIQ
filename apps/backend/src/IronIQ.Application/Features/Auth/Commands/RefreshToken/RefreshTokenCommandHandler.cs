using System.Security.Cryptography;
using System.Text;
using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Auth.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand command, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, ct);
        if (user is null)
            return Result<AuthResponseDto>.Failure(Error.Unauthorized());

        var tokenHash = ComputeSha256(command.RefreshToken);
        if (!user.IsRefreshTokenValid(tokenHash))
            return Result<AuthResponseDto>.Failure(Error.Unauthorized("Refresh token is invalid or expired."));

        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email);
        var (newRefreshToken, newHash, newExpiry) = jwtService.GenerateRefreshToken();
        user.SetRefreshToken(newHash, newExpiry);

        await unitOfWork.SaveChangesAsync(ct);

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(accessToken, newRefreshToken, user.Id, user.Email));
    }

    private static string ComputeSha256(string input)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
}
