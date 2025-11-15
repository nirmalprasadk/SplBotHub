namespace SplBotHub.Services;

public interface ISboxConnectionService
{
    Task ToggleSBoxConnection(bool IsConnectionActive);
}