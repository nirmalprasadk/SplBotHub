using Reusables.Contracts;
using Reusables.Models;

namespace Reusables.Services.Connection;

public class SBoxConnectionService : ISboxClient
{
    private readonly SBoxServerConfig _sboxServerConfig;
    private readonly IClient _client;

    public bool IsConnected { get; private set; }

    public SBoxConnectionService(IClient client)
    {
        _client = client;
        _sboxServerConfig = ConfigService.GetAppConfig().SBOXServerConfig;
    }

    public async Task ConnectAsync()
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

    public async Task DisconnectAsync()
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

    public async Task ToggleConnectionAsync()
    {
        if (IsConnected)
        {
            await DisconnectAsync();
        }
        else
        {
            await ConnectAsync();
        }
    }

    public async Task SendMessageAsync(string message)
    {
        await _client.SendMessageAsync(message, CancellationToken.None);
    }
}
