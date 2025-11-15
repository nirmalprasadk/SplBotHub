using BotContract.Interfaces;

namespace Bots;

public abstract class BotBase : IBot
{
    protected readonly IClient GameConnection;

    public string Name { get; }

    protected BotBase(IClient gameConnection, string? name = null)
    {
        GameConnection = gameConnection;
        Name = name ?? GetType().Name;
    }

    public virtual void Start()
    {
        GameConnection.OnMessageReceived += GameEventReceived;
    }

    public virtual void Stop()
    {
        GameConnection.OnMessageReceived -= GameEventReceived;
    }

    protected abstract void GameEventReceived(string message);
}
