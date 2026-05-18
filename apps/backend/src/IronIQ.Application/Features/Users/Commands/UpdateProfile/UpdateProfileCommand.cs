using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Users.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(
    string? Name,
    int? Age,
    float? HeightCm,
    float? WeightKg,
    string? Goal,
    string? Level,
    bool? IsLeaderboardOptIn) : IRequest<Result<UserProfileDto>>;
