using Reusables.Contracts;
using Reusables.Enums;
using Reusables.Models;
using Reusables.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace BotHub.Services;

public class SBoxClient : ISboxClient
{
    private readonly SBoxServerConfig _sboxServerConfig;
    private readonly IClient _client;
    private Action<string>? _messageReceived;

    public event Action<string>? MessageReceived
    {
        add
        {
            _messageReceived += value;
            Log(MessageSource.Bot, $"{value?.Target} connected.");
        }
        remove
        {
            _messageReceived -= value;
            Log(MessageSource.Bot, $"{value?.Target} disconnected.");
        }
    }

    public bool IsConnected { get; private set; }

    public ObservableCollection<BotLogEntry> Logs { get; }

    public SBoxClient(IClient client)
    {
        _client = client;
        _sboxServerConfig = ConfigService.GetAppConfig().SBOXServerConfig;

        Logs = new ObservableCollection<BotLogEntry>();
        Log(MessageSource.App, "SBOX service initialized");
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
        finally
        {
            Log(MessageSource.App, IsConnected ? "Connected to SBOX client." : "Failed to connect to SBOX client.");
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
            Log(MessageSource.App, "Disconnected from SBOX server.");
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
        Log(MessageSource.Bot, message);
        await _client.SendMessageAsync(message, CancellationToken.None);
    }

    public void ClearLogs()
    {
        Logs.Clear();
    }

    public void Log(MessageSource source, string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Logs.Insert(0, new BotLogEntry(source, message));
        });
    }

    private void StartConsumingMessageQueue()
    {
        var reader = _client.Messages;

        _ = Task.Run(async () =>
        {
            await foreach (string message in reader.ReadAllAsync())
            {
                Log(MessageSource.Game, message);
                _messageReceived?.Invoke(message);
            }
        });
    }
}
