namespace BotContract.Interfaces;

public interface IClient
{
    Task ConnectAsync(Uri serverUri, CancellationToken cancellationToken);

    Task DisconnectAsync(CancellationToken cancellationToken);

    Task SendMessageAsync(string message, CancellationToken cancellationToken);

    event Action<string>? OnMessageReceived;
}