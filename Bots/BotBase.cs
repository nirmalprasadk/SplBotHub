using BotContract.Interfaces;

namespace Bots;

public abstract class BotBase : IBot
{
    protected readonly IGameConnection GameConnection;

    public string Name { get; }

    protected BotBase(IGameConnection gameConnection, string? name = null)
    {
        GameConnection = gameConnection;
        Name = name ?? GetType().Name;
    }

    public virtual void Start()
    {
        GameConnection.OnGameEventReceived += GameEventReceived;
    }

    public virtual void Stop()
    {
        GameConnection.OnGameEventReceived -= GameEventReceived;
    }

    protected abstract void GameEventReceived(string message);
}
