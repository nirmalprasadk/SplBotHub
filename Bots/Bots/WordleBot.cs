using Reusables.Contracts;

namespace Bots.Bots;

public class WordleBot(ISboxClient sBoxClient, IAIService aIService, string? name = null) : BotBase(sBoxClient, aIService, name)
{
    protected override void OnSBoxMessageReceived(string message)
    {
        throw new NotImplementedException();
    }
}