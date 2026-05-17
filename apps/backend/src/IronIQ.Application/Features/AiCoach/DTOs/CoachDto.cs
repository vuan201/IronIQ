namespace IronIQ.Application.Features.AiCoach.DTOs;

public record CoachMessageDto(string Role, string Content);

public record CoachResponseDto(string Reply);
