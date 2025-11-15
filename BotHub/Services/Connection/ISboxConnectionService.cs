namespace BotHub.Services.Connection;

public interface ISboxConnectionService
{
    Task ToggleConnection(bool IsConnectionActive);
}