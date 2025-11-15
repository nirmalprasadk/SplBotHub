namespace SplBotHub.Services.Connection;

public interface ISboxConnectionService
{
    Task ToggleSBoxConnection(bool IsConnectionActive);
}