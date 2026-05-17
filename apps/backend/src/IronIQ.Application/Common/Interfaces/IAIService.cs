namespace IronIQ.Application.Common.Interfaces;

public interface IAIService
{
    Task<string> CompleteAsync(string prompt, int maxTokens = 4096, CancellationToken ct = default);
}
