using System.Net.Http.Json;
using System.Text.Json;
using IronIQ.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IronIQ.Infrastructure.External.Claude;

public class ClaudeAIService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IAIService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<string> CompleteAsync(string prompt, int maxTokens = 4096, CancellationToken ct = default)
    {
        var apiKey = configuration["Claude:ApiKey"] ?? throw new InvalidOperationException("Claude:ApiKey not configured.");
        var model = configuration["Claude:Model"] ?? "claude-sonnet-4-6";

        var requestBody = new
        {
            model,
            max_tokens = maxTokens,
            messages = new[] { new { role = "user", content = prompt } }
        };

        var client = httpClientFactory.CreateClient("claude");
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");
        request.Content = JsonContent.Create(requestBody, options: JsonOptions);

        var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        return json.GetProperty("content")[0].GetProperty("text").GetString() ?? string.Empty;
    }
}
