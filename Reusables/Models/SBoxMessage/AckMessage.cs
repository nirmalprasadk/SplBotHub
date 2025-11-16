namespace Reusables.Models.SBoxMessage;

public class AckMessage : SBoxMessageBase
{
    public string? AckFor { get; set; }

    public string? AckData { get; set; }
}
