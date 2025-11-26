using Reusables.Contracts;
using Reusables.Enums;
using Reusables.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Reusables.Services.AI;

public class OpenAIService : IAIService
{
    private readonly AIConfig _aiConfig;

    private static readonly HttpClient _httpClient = new()
    {
        Timeout = Timeout.InfiniteTimeSpan
    };

    private const string ResponsesApiUrl = "https://api.openai.com/v1/responses";

    public OpenAIService()
    {
        _aiConfig = ConfigService.GetAppConfig().AIConfig;
    }

    public async Task<string> AskAsync(string prompt, CancellationTokenSource cts)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(_aiConfig.ApiKey))
            throw new InvalidOperationException("OpenAI API key is not configured.");

        var requestJson = BuildRequestJson(prompt, _aiConfig.DefaultModel.ToModelString());

        const int maxRetries = 3;
        TimeSpan attemptTimeout = TimeSpan.FromSeconds(60);

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
                timeoutCts.CancelAfter(attemptTimeout);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, ResponsesApiUrl)
                {
                    Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
                };

                httpRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _aiConfig.ApiKey);
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var response = await _httpClient.SendAsync(
                    httpRequest,
                    HttpCompletionOption.ResponseHeadersRead,
                    timeoutCts.Token
                );

                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Retry on transient errors
                    if (ShouldRetry(response.StatusCode) && attempt < maxRetries)
                    {
                        await DelayRetry(attempt, cts.Token);
                        continue;
                    }

                    return string.Empty;
                }

                return ParseResponse(responseString);
            }
            catch (OperationCanceledException) when (!cts.IsCancellationRequested)
            {
                // Timeout
                if (attempt == maxRetries) return string.Empty;
                await DelayRetry(attempt, cts.Token);
            }
            catch (HttpRequestException)
            {
                if (attempt == maxRetries) return string.Empty;
                await DelayRetry(attempt, cts.Token);
            }
        }

        return string.Empty;
    }

    // ------------------------------
    //  Helpers
    // ------------------------------

    private static string BuildRequestJson(string prompt, string model)
    {
        var requestBody = new
        {
            model,
            input = new[]
            {
                new { role = "user", content = prompt }
            },
            reasoning = new { effort = "low" }
        };

        return JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private static bool ShouldRetry(System.Net.HttpStatusCode statusCode)
        => statusCode == System.Net.HttpStatusCode.TooManyRequests
           || (int)statusCode >= 500;

    private static Task DelayRetry(int attempt, CancellationToken token)
    {
        int seconds = (int)Math.Pow(2, attempt); // 2, 4, 8
        return Task.Delay(TimeSpan.FromSeconds(seconds), token);
    }

    private static string ParseResponse(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Preferred: Responses API → output[].content[].text
            if (root.TryGetProperty("output", out var output) && output.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in output.EnumerateArray())
                {
                    if (item.TryGetProperty("content", out var content) &&
                        content.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var c in content.EnumerateArray())
                        {
                            if (c.TryGetProperty("text", out var textProp) &&
                                textProp.ValueKind == JsonValueKind.String)
                                return textProp.GetString() ?? "";
                        }
                    }
                }
            }

            // Fallback: Chat Completions-style
            if (root.TryGetProperty("choices", out var choices) &&
                choices.ValueKind == JsonValueKind.Array &&
                choices.GetArrayLength() > 0)
            {
                var first = choices[0];

                if (first.TryGetProperty("message", out var msg) &&
                    msg.TryGetProperty("content", out var contentEl) &&
                    contentEl.ValueKind == JsonValueKind.String)
                    return contentEl.GetString() ?? "";
            }
        }
        catch
        {
            return string.Empty;
        }

        return string.Empty;
    }
}
