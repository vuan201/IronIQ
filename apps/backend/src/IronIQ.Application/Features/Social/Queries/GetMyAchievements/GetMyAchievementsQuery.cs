using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.Social.DTOs;
using MediatR;

namespace IronIQ.Application.Features.Social.Queries.GetMyAchievements;

public record GetMyAchievementsQuery : IRequest<Result<IList<AchievementDto>>>;
