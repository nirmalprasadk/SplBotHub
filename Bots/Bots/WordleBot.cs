using Reusables.Contracts;
using Reusables.Models.SBoxMessage;

namespace Bots.Bots;

public class WordleBot(ISboxClient sBoxClient, IAIService aIService, string? name = null) : BotBase(sBoxClient, aIService, name)
{
    protected override void HandleAck(AckMessage ack)
    {
        Console.WriteLine($"Received Ack for: {ack.AckFor}");
    }

    protected override void HandleCommand(CommandMessage command)
    {
    }

    protected override void HandleGameResult(GameResultMessage gameResult)
    {
        Console.WriteLine("========================================");
        Console.WriteLine($" Result for match {gameResult.MatchId}");
        Console.WriteLine($" Game:     {gameResult.GameId}");
        Console.WriteLine($" Outcome:  {gameResult.Result?.ToUpper()}");
        Console.WriteLine($" Word:     {gameResult.Word}");
        Console.WriteLine("========================================");
    }
}