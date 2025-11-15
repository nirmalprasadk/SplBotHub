using BotContract.Interfaces;

namespace Bots.Bots;

public class CluedleBot(IGameConnection gameConnection, string? name = null) : BotBase(gameConnection, name)
{
    protected override void GameEventReceived(string message)
    {
        throw new NotImplementedException();
    }
}
