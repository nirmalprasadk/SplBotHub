using Reusables.Models.SBoxMessage;
using System.Text.Json;

namespace Reusables.Parsers;

public static class SBoxMessageParser
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static SBoxMessageBase? Parse(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        if (!root.TryGetProperty("type", out JsonElement typeProp))
        {
            return null;
        }

        string type = typeProp.GetString()!.Trim().ToLower();

        return type switch
        {
            "ack" => JsonSerializer.Deserialize<AckMessage>(json, _options)!,
            "command" => JsonSerializer.Deserialize<CommandMessage>(json, _options)!,
            "game result" => JsonSerializer.Deserialize<GameResultMessage>(json, _options)!,
            _ => null
        };
    }

    public static string Serialize(BotGuessMessage response)
    {
        return JsonSerializer.Serialize(response, _options);
    }
}
