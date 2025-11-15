using Reusables.Contracts;
using Reusables.Enums;
using Reusables.Models;
using System.Collections.ObjectModel;

namespace Bots;

public abstract class BotBase : IBot
{
    protected readonly IClient GameConnection;

    public string Name { get; }

    public bool IsRunning { get; private set; }

    public ObservableCollection<BotLogEntry> SessionLogs {  get; }

    protected BotBase(IClient gameConnection, string? name = null)
    {
        GameConnection = gameConnection;
        Name = name ?? GetType().Name;

        SessionLogs = new ObservableCollection<BotLogEntry>();
    }

    public virtual void Start()
    {
        SessionLogs.Clear();
        GameConnection.OnMessageReceived += OnGameEventReceivedInternal;
        IsRunning = true;
    }

    public virtual void Stop()
    {
        GameConnection.OnMessageReceived -= OnGameEventReceivedInternal;
        IsRunning = false;
    }

    public void ToggleConnection()
    {
        if (IsRunning)
        {
            Stop();
        }
        else
        {
           Start();
        }
    }

    protected async Task SendEventToGameInternal(string message)
    {
        SessionLogs.Add(new BotLogEntry(MessageSource.Bot, message));
        await GameConnection.SendMessageAsync(message, CancellationToken.None);
    }

    async Task IBot.SendEventToGame(string message)
    {
        await SendEventToGameInternal(message);
    }

    private void OnGameEventReceivedInternal(string message)
    {
        SessionLogs.Add(new BotLogEntry(MessageSource.Game, message));
        GameEventReceived(message);
    }

    protected abstract void GameEventReceived(string message);
}
