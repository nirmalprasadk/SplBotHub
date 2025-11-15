using BotContract.Interfaces;
using Bots;

namespace SplBotHub.Bots;

public class WordleBot(IGameConnection gameConnection, string? name = null) : BotBase(gameConnection, name)
{
    protected override void GameEventReceived(string message)
    {
        throw new NotImplementedException();
    }
}