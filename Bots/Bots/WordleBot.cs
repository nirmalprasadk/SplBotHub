using Reusables.Contracts;

namespace Bots.Bots;

public class WordleBot(IClient gameConnection, string? name = null) : BotBase(gameConnection, name)
{
    protected override void GameEventReceived(string message)
    {
        throw new NotImplementedException();
    }
}