using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.GenerateAIPlan;

public record GenerateAIPlanCommand(
    string Goal,
    string FitnessLevel,
    int DaysPerWeek,
    IList<string> AvailableEquipment,
    IList<string> FocusAreas) : IRequest<Result<WorkoutPlanDto>>;
