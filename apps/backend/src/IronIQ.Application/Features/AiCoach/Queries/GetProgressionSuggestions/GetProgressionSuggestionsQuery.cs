using IronIQ.Application.Common.Models;
using IronIQ.Application.Features.AiCoach.DTOs;
using MediatR;

namespace IronIQ.Application.Features.AiCoach.Queries.GetProgressionSuggestions;

public record GetProgressionSuggestionsQuery(Guid SessionId) : IRequest<Result<List<ProgressionSuggestionDto>>>;
