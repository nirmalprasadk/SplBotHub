namespace BotHub.Services.Connection;

public interface ISboxConnectionService
{
    Task ToggleSBoxConnection(bool IsConnectionActive);
}