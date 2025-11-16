using OpenAI;
using OpenAI.Chat;
using Reusables.Contracts;
using Reusables.Enums;
using Reusables.Models;

namespace Reusables.Services.AI;

public class OpenAIService : IAIService
{
    private readonly AIConfig _aiConfig;
    private readonly OpenAIClient _client;

    public OpenAIService()
    {
        _aiConfig = ConfigService.GetAppConfig().AIConfig;
        _client = new OpenAIClient(_aiConfig.ApiKey);
    }

    public async Task<string> AskAsync(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return string.Empty;
        }

        string modelName = _aiConfig.DefaultModel.ToModelString();
        ChatClient chat = _client.GetChatClient(modelName);

        var result = await chat.CompleteChatAsync(
            messages:
            [
                ChatMessage.CreateUserMessage(prompt)
            ]
        );

        ChatCompletion? message = result?.Value;

        if (message?.Content is null || !message.Content.Any())
        {
            return string.Empty;
        }

        return message.Content.First().Text;
    }
}