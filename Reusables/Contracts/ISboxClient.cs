namespace Reusables.Contracts;

public interface ISboxClient
{
    bool IsConnected { get; }

    Task ConnectAsync();

    Task DisconnectAsync();

    Task ToggleConnectionAsync();

    Task SendMessageAsync(string message);
}