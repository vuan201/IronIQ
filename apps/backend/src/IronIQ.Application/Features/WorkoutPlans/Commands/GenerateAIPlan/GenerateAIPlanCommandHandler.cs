using System.Text.Json;
using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.WorkoutPlans.DTOs;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using MediatR;

namespace IronIQ.Application.Features.WorkoutPlans.Commands.GenerateAIPlan;

public class GenerateAIPlanCommandHandler(
    IAIService aiService,
    IWorkoutPlanRepository planRepository,
    IExerciseRepository exerciseRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<GenerateAIPlanCommand, Result<WorkoutPlanDto>>
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Result<WorkoutPlanDto>> Handle(GenerateAIPlanCommand command, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<WorkoutPlanDto>.Failure(Error.Unauthorized());

        var user = await userRepository.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null) return Result<WorkoutPlanDto>.Failure(Error.Unauthorized());

        if (user.SubscriptionTier == SubscriptionTier.Free)
        {
            var aiCount = await planRepository.CountAIGeneratedThisMonthAsync(currentUser.UserId.Value, ct);
            if (aiCount >= 3)
                return Result<WorkoutPlanDto>.Failure(Error.Conflict("Free tier allows 3 AI-generated plans per month. Upgrade to Premium for unlimited plans."));
        }

        var prompt = PromptBuilder.BuildGeneratePlanPrompt(command);
        var rawJson = await aiService.CompleteAsync(prompt, maxTokens: 2048, ct: ct);

        AIPlanResponse? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<AIPlanResponse>(rawJson.Trim(), JsonOptions);
        }
        catch
        {
            return Result<WorkoutPlanDto>.Failure(Error.Failure("AI returned an unreadable response. Please try again."));
        }

        if (parsed is null)
            return Result<WorkoutPlanDto>.Failure(Error.Failure("AI returned an empty response."));

        var plan = WorkoutPlan.Create(currentUser.UserId.Value, parsed.Name, parsed.Description, isAIGenerated: true);

        var days = new List<WorkoutDay>();
        foreach (var d in parsed.Days)
        {
            var day = WorkoutDay.Create(plan.Id, d.DayOfWeek, d.Name);
            var exercises = new List<PlanExercise>();

            int order = 1;
            foreach (var ex in d.Exercises)
            {
                var existing = await exerciseRepository.FindByNameAsync(ex.ExerciseName, ct);
                if (existing is null)
                {
                    existing = Exercise.CreateUserDefined(
                        currentUser.UserId.Value, ex.ExerciseName, null,
                        [MuscleGroup.FullBody], [Equipment.Bodyweight], Difficulty.Intermediate);
                    await exerciseRepository.AddAsync(existing, ct);
                }

                exercises.Add(PlanExercise.Create(day.Id, existing.Id, order++, ex.Sets, ex.Reps, ex.DurationSeconds, ex.RestSeconds, ex.Notes));
            }

            day.Exercises.AddRange(exercises);
            days.Add(day);
        }

        plan.ReplaceDays(days);
        await planRepository.AddAsync(plan, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<WorkoutPlanDto>.Success(MapToDto(plan));
    }

    private static WorkoutPlanDto MapToDto(WorkoutPlan plan) => new(
        plan.Id,
        plan.Name,
        plan.Description,
        plan.IsActive,
        plan.CreatedAt,
        plan.Days.Select(d => new WorkoutDayDto(
            d.Id,
            d.DayOfWeek,
            d.Name,
            d.Exercises.Select(e => new PlanExerciseDto(
                e.Id, e.ExerciseId, e.Exercise?.Name ?? string.Empty,
                e.Order, e.Sets, e.Reps, e.DurationSeconds, e.RestSeconds, e.Notes)
            ).ToList()
        )).ToList());
}

internal record AIPlanResponse(string Name, string? Description, List<AIDayResponse> Days);
internal record AIDayResponse(int DayOfWeek, string? Name, List<AIExerciseResponse> Exercises);
internal record AIExerciseResponse(string ExerciseName, int Order, int Sets, int? Reps, int? DurationSeconds, int RestSeconds, string? Notes);
