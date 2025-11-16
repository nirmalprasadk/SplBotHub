using Reusables.Models.SBoxMessage;

namespace Reusables.Contracts;

public interface IBot
{
    string Name { get; }

    bool IsRunning { get; }

    void Start();

    void Stop();

    void ToggleConnection();

    Task SendMessageToSBox(BotGuessMessage response);
}