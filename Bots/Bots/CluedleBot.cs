using Reusables.Contracts;

namespace Bots.Bots;

public class CluedleBot(IClient gameConnection, IAIService aIService, string? name = null) : BotBase(gameConnection, aIService, name)
{
    protected override void GameEventReceived(string message)
    {
        throw new NotImplementedException();
    }
}
