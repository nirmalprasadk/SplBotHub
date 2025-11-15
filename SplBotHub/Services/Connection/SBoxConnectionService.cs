using BotContract.Interfaces;

namespace SplBotHub.Services.Connection;

public class SBoxConnectionService : ISboxConnectionService
{
    private readonly AppConfig _appConfig;
    private readonly IClient _client;

    public SBoxConnectionService(IClient client)
    {
        _appConfig = ConfigService.Load() ?? new();

        _client = client;
    }

    public async Task ToggleSBoxConnection(bool IsConnectionActive)
    {
        if (IsConnectionActive)
        {
            await _client.DisconnectAsync(CancellationToken.None);
        }
        else
        {
            Uri? serverUri = _appConfig.SBOXServerConfig?.BuildUri()
                ?? throw new NullReferenceException("Server URI is null. Please check your configuration.");

            await _client.ConnectAsync(serverUri, CancellationToken.None);
        }
    }
}
