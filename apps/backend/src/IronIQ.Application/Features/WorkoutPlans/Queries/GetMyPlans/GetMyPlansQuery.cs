using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Queries.GetMyPlans;

public record GetMyPlansQuery : IRequest<Result<List<WorkoutPlanDto>>>;
