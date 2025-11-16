using Reusables.Contracts;
using Reusables.Enums;
using Reusables.Models;
using Reusables.Services;
using System.Collections.ObjectModel;

namespace BotHub.Services;

public class SBoxConnectionService : ISboxClient
{
    private readonly SBoxServerConfig _sboxServerConfig;
    private readonly IClient _client;

    public event Action<string>? MessageReceived;

    public bool IsConnected { get; private set; }

    public ObservableCollection<BotLogEntry> Logs { get; }

    public SBoxConnectionService(IClient client)
    {
        _client = client;
        _sboxServerConfig = ConfigService.GetAppConfig().SBOXServerConfig;

        Logs = new ObservableCollection<BotLogEntry>();
    }

    public async Task ConnectAsync()
    {
        try
        {
            Uri serverUri = _sboxServerConfig.BuildUri();
            await _client.ConnectAsync(serverUri, CancellationToken.None);

            StartConsumingMessageQueue();

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

    private void StartConsumingMessageQueue()
    {
        var reader = _client.Messages;

        _ = Task.Run(async () =>
        {
            await foreach (string msg in reader.ReadAllAsync())
            {
                MessageReceived?.Invoke(msg);
            }
        });
    }

    private void Log(MessageSource source, string message)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            Logs.Add(new BotLogEntry(source, message));
        });
    }
}
