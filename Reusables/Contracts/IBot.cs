using Reusables.Models;
using System.Collections.ObjectModel;

namespace Reusables.Contracts;

public interface IBot
{
    string Name { get; }

    void Start();

    void Stop();

    void ToggleConnection();

    bool IsRunning { get; }

    Task SendEventToGame(string message);

    ObservableCollection<BotLogEntry> SessionLogs { get; }
}