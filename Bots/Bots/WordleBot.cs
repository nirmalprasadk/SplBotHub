using BotContract.Interfaces;
using Bots;

namespace SplBotHub.Bots;

public class WordleBot(IClient gameConnection, string? name = null) : BotBase(gameConnection, name)
{
    protected override void GameEventReceived(string message)
    {
        throw new NotImplementedException();
    }
}