namespace IronIQ.Application.Common.Interfaces;

public interface IAIService
{
    Task<string> CompleteAsync(string prompt, int maxTokens = 4096, CancellationToken ct = default);
    Task<string> ChatAsync(string systemPrompt, IList<ChatMessage> messages, int maxTokens = 1024, CancellationToken ct = default);
}

public record ChatMessage(string Role, string Content);
