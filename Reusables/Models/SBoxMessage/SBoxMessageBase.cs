namespace Reusables.Models.SBoxMessage;

public abstract class SBoxMessageBase
{
    public string? MatchId { get; set; }

    public string? GameId { get; set; }

    public string? YourId { get; set; }

    public string? Type { get; set; }
}
