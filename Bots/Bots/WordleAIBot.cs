using Reusables.Contracts;
using Reusables.Models.Game;
using Reusables.Models.SBoxMessage;
using System.Text;

namespace Bots.Bots;

public class WordleAIBot(ISboxClient sBoxClient, IAIService aIService, string? name = null) : BotBase(sBoxClient, aIService, name)
{
    private const string fallbackWord = "AEIOU";
    private readonly Dictionary<string, WordleGameState> _games = new();

    protected override void HandleAck(AckMessage ack)
    {
    }

    protected override async void HandleCommand(CommandMessage command)
    {
        // STEP 1 → Ensure game state exists
        if (!_games.TryGetValue(command.GameId!, out WordleGameState? state))
        {
            state = new WordleGameState
            {
                MatchId = command.MatchId,
                GameId = command.GameId,
                WordLength = command.WordLength
            };

            _games[command.GameId!] = state;
        }

        // STEP 2 → Record previous guess + result
        if (!string.IsNullOrWhiteSpace(command.LastGuess))
        {
            state.Guesses.Add(command.LastGuess);
            state.Results.Add(command.LastResult!);
        }

        // STEP 3 → Build AI prompt using ENTIRE HISTORY
        string prompt = BuildPromptFromHistory(state, command);

        // STEP 4 → Ask AI service for the next guess
        string guess = string.Empty;
        try
        {
            guess = await AIService.AskAsync(prompt, new(TimeSpan.FromSeconds(50)));
            guess = guess.Trim().ToUpper();
        }
        catch (Exception ex)
        {
            Log($"MatchID:{command.MatchId}; " +
                $"GameID: {command.GameId}; " +
                $"Error: {ex.Message}; " +
                $"FallbackWord: {fallbackWord}");
            guess = fallbackWord;
        }

        // STEP 5 → Construct response message
        BotGuessMessage botGuessMessage = new()
        {
            MatchId = command.MatchId,
            GameId = command.GameId,
            Otp = command.Otp,
            Guess = guess
        };

        // STEP 6 → Send to server
        IBot bot = this;
        await bot.SendMessageToSBox(botGuessMessage);
    }

    private string BuildPromptFromHistory(WordleGameState state, CommandMessage current)
    {
        StringBuilder sb = new();

        sb.AppendLine("You are solving a Wordle game.");
        sb.AppendLine($"Word length: {state.WordLength}");
        sb.AppendLine($"Attempt number: {current.CurrentAttempt}");
        sb.AppendLine($"Max attempts: {current.MaxAttempts}");
        sb.AppendLine();
        sb.AppendLine("Here is the complete guess history:");

        if (state.Guesses.Count == 0)
        {
            sb.AppendLine("No guesses yet. This is the first move. Pick a word that could maximize the change of winning.");
        }
        else
        {
            for (int i = 0; i < state.Guesses.Count; i++)
            {
                string guess = state.Guesses[i];
                string result = string.Join(", ", state.Results[i]);

                sb.AppendLine($"Guess {i + 1}: {guess}");
                sb.AppendLine($"Result {i + 1}: {result}");
                sb.AppendLine();
            }
        }

        sb.AppendLine("Objective: Minimize the number of attempts.");
        sb.AppendLine("Follow Wordle rules:");
        sb.AppendLine("- 'Correct'  = correct letter in correct position");
        sb.AppendLine("- 'Present'  = letter exists but wrong position");
        sb.AppendLine("- 'Absent'   = letter does NOT exist in the word");
        sb.AppendLine();
        sb.AppendLine($"Return ONLY a single English word with exactly {state.WordLength} letters.");

        return sb.ToString();
    }

    protected override void HandleGameResult(GameResultMessage gameResult)
    {
        _games.Remove(gameResult.GameId!);
    }
}
