using IronIQ.Application.Common.Interfaces;
using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using MediatR;

namespace IronIQ.Application.Features.AiCoach.Queries.GetProgressionSuggestions;

public class GetProgressionSuggestionsQueryHandler(
    IProgressionSuggestionRepository repository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetProgressionSuggestionsQuery, Result<List<ProgressionSuggestionDto>>>
{
    public async Task<Result<List<ProgressionSuggestionDto>>> Handle(
        GetProgressionSuggestionsQuery query, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<List<ProgressionSuggestionDto>>.Failure(Error.Unauthorized());

        var suggestions = await repository.GetBySessionIdAsync(query.SessionId, ct);
        var dtos = suggestions
            .Select(s => new ProgressionSuggestionDto(
                s.ExerciseId, s.ExerciseName, s.CurrentWeightKg, s.SuggestedWeightKg))
            .ToList();

        return Result<List<ProgressionSuggestionDto>>.Success(dtos);
    }
}
