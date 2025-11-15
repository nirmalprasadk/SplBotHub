namespace SplBotHub.Connection;

public interface IGameConnection
{
    Task ConnectAsync(Uri serverUri, CancellationToken cancellationToken);

    Task DisconnectAsync(CancellationToken cancellationToken);
}
