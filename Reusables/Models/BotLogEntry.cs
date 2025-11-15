using Reusables.Enums;

namespace Reusables.Models;

public class BotLogEntry
{
    public MessageSource MessageSource { get; set; }

    public string Message { get; set; }

    public BotLogEntry(MessageSource messageSource, string message)
    {
        MessageSource = messageSource;
        Message = message;
    }
}
