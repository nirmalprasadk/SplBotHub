using Reusables.Models;
using System.Collections.ObjectModel;

namespace Reusables.Contracts;

public interface ISboxClient
{
    bool IsConnected { get; }

    Task ConnectAsync();

    Task DisconnectAsync();

    Task ToggleConnectionAsync();

    Task SendMessageAsync(string message);

    event Action<string>? MessageReceived;

    ObservableCollection<BotLogEntry> Logs { get; }
}