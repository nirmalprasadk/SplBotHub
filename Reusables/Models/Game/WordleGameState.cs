namespace Reusables.Models.Game;

public class WordleGameState
{
    public string? MatchId { get; set; }

    public string? GameId { get; set; }

    public int WordLength { get; set; }

    public List<string> Guesses { get; set; }

    public List<List<string>> Results { get; set; }

    public WordleGameState()
    {
        Guesses = new List<string>();
        Results = new List<List<string>>();
    }
}
