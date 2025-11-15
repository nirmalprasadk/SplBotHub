namespace SplBotHub.Bots;

public class WordleBot : IBot
{
    public string Name { get; }

    public WordleBot()
    {
        Name = nameof(WordleBot);
    }
}
