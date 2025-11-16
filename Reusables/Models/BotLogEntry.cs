using Reusables.Enums;
using Reusables.Models.SBoxMessage;
using System.Text.Json;

namespace Reusables.Models;

public class BotLogEntry
{
    public MessageSource MessageSource { get; set; }

    public DateTime LogTime { get; set; }

    public string Message { get; set; }

    public string? GameId { get; set; }

    public BotLogEntry(MessageSource messageSource, string message)
    {
        MessageSource = messageSource;
        LogTime = DateTime.Now;
        Message = message;
        GameId = TryExtractGameId(message);
    }

    private string? TryExtractGameId(string message)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(message);

            if (doc.RootElement.TryGetProperty("gameId", out JsonElement gameIdProp))
            {
                return gameIdProp.GetString();
            }
        }
        catch
        {
            // ignore malformed JSON
        }

        return null;
    }
}
