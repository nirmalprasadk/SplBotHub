namespace Reusables.Models.Game;

public class WordleGameState
{
    public string? MatchId { get; set; }

    public string? GameId { get; set; }

    public int WordLength { get; set; }

    public List<string> Guesses { get; set; } = new();

    public List<List<string>> Results { get; set; } = new();
}
