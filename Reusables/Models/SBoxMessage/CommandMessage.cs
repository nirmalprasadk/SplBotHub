using System.Text.Json;

namespace Reusables.Models.SBoxMessage;

public class CommandMessage : SBoxMessageBase
{
    public string? Otp { get; set; }

    public string? Command { get; set; }

    public int WordLength { get; set; }

    public int MaxAttempts { get; set; }

    public string? LastGuess { get; set; }

    public List<string>? LastResult { get; set; }

    public int CurrentAttempt { get; set; }
}

