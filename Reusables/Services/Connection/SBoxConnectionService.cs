using Reusables.Contracts;
using Reusables.Models;

namespace Reusables.Services.Connection;

public class SBoxConnectionService : ISboxConnectionService
{
    private readonly SBoxServerConfig _sboxServerConfig;
    private readonly IClient _client;

    public bool IsConnected { get; private set; }

    public SBoxConnectionService(IClient client)
    {
        _client = client;
        _sboxServerConfig = ConfigService.GetAppConfig().SBOXServerConfig;
    }

    public async Task Connect()
    {
        try
        {
            Uri serverUri = _sboxServerConfig.BuildUri();
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
