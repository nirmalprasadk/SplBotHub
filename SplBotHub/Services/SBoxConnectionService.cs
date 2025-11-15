using SplBotHub.Connection;

namespace SplBotHub.Services;

public class SBoxConnectionService : ISboxConnectionService
{
    private readonly AppConfig _appConfig;
    private readonly IGameConnection _gameConnection;

    public SBoxConnectionService(IGameConnection gameConnection)
    {
        _appConfig = ConfigService.Load() ?? new();

        _gameConnection = gameConnection;
    }

    public async Task ToggleSBoxConnection(bool IsConnectionActive)
    {
        if (IsConnectionActive)
        {
            await _gameConnection.DisconnectAsync(CancellationToken.None);
        }
        else
        {
            Uri? serverUri = _appConfig.GameConnection?.BuildUri()
                ?? throw new NullReferenceException("Server URI is null. Please check your configuration.");

            await _gameConnection.ConnectAsync(serverUri, CancellationToken.None);
        }
    }
}
