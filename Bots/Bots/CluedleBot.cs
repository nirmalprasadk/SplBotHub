using BotContract.Interfaces;

namespace Bots.Bots;

public class CluedleBot(IClient gameConnection, string? name = null) : BotBase(gameConnection, name)
{
    protected override void GameEventReceived(string message)
    {
        throw new NotImplementedException();
    }
}
