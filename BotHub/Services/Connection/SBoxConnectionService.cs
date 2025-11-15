using BotContract.Interfaces;

namespace BotHub.Services.Connection;

public class SBoxConnectionService : ISboxConnectionService
{
    private readonly AppConfig _appConfig;
    private readonly IClient _client;

    public bool IsConnected { get; private set; }

    public SBoxConnectionService(IClient client)
    {
        _appConfig = ConfigService.Load() ?? new();

        _client = client;
    }

    public async Task Connect()
    {
        try
        {
            Uri? serverUri = _appConfig.SBOXServerConfig?.BuildUri()
                ?? throw new NullReferenceException("Server URI is null. Please check your configuration.");

            await _client.ConnectAsync(serverUri, CancellationToken.None);
            IsConnected = true;
        }
        catch (Exception)
        {
            IsConnected = false;
        }
    }

    public async Task Disconnect()
    {
        try
        {
            await _client.DisconnectAsync(CancellationToken.None);
        }
        catch (Exception)
        {
            // Ignore exceptions during disconnect
        }
        finally
        {
            IsConnected = false;
        }
    }

    public async Task ToggleConnection()
    {
        if (IsConnected)
        {
            await Disconnect();
        }
        else
        {
            await Connect();
        }
    }
}
