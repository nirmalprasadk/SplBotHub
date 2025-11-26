using Reusables.Enums;
using Reusables.Models;
using System.Collections.ObjectModel;

namespace Reusables.Contracts;

public interface ISboxClient
{
    bool IsConnected { get; }

    event Action<string>? MessageReceived;

    ObservableCollection<BotLogEntry> Logs { get; }

    Task ConnectAsync();

    Task DisconnectAsync();

    Task ToggleConnectionAsync();

    Task SendMessageAsync(string message);

    void Log(MessageSource source, string message);

    void ClearLogs();
}