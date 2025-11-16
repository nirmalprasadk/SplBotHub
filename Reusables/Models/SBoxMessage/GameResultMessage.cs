namespace Reusables.Models.SBoxMessage;

public class GameResultMessage : SBoxMessageBase
{
    public string? Result { get; set; }

    public string? Word { get; set; }
}