namespace BotContract.Interfaces;

public interface IGameConnection
{
    Task ConnectAsync(Uri serverUri, CancellationToken cancellationToken);

    Task DisconnectAsync(CancellationToken cancellationToken);

    void SendMessage(string message);

    event Action<string>? OnGameEventReceived;
}