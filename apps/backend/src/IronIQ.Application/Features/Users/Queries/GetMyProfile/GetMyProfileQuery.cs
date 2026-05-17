using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Users.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Users.Queries.GetMyProfile;

public record GetMyProfileQuery : IRequest<Result<UserProfileDto>>;
