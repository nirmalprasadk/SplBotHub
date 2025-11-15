namespace Reusables.Services.Connection;

public interface ISboxConnectionService
{
    Task Connect();

    Task Disconnect();

    Task ToggleConnection();

    bool IsConnected { get; }
}