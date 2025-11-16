using Reusables.Contracts;
using System.Collections.ObjectModel;
using Reusables.Models;
using System.Windows.Input;

namespace BotHub.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ISboxClient _sBoxClient;
    private readonly IBotLoaderService _botLoaderService;
    private IBot? _selectedBot;

    public string WindowTitle => "SPL Bot Hub";

    public bool IsSBoxConnected => _sBoxClient.IsConnected;

    public BindingCommand SBoxConnectionCommand { get; private set; }

    public BindingCommand ReloadBotsCommand { get; private set; }

    public BindingCommand BotConnectionCommand { get; private set; }

    public bool IsBotRunning => SelectedBot is not null && SelectedBot.IsRunning;

    public ObservableCollection<IBot> AvailableBots { get; }

    public IBot? SelectedBot
    {
        get => _selectedBot; 
        set
        {
            _selectedBot = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SBoxLogs));
            OnPropertyChanged(nameof(CanDisplayLogs));
        }
    }

    public bool CanDisplayLogs => SelectedBot is not null;

    public ObservableCollection<BotLogEntry> SBoxLogs => _sBoxClient.Logs;

    public MainWindowViewModel(ISboxClient sBoxClient, IBotLoaderService botLoaderService)
    {
        _sBoxClient = sBoxClient;
        _botLoaderService = botLoaderService;

        SBoxConnectionCommand = new BindingCommand(UpdateSBoxConnection);
        ReloadBotsCommand = new BindingCommand(ReloadBots, CanReloadBots);
        BotConnectionCommand = new BindingCommand(UpdateBotConnection, CanEnableBotConnectionButton);

        AvailableBots = new ObservableCollection<IBot>();
        LoadBotsToUI();
    }

    private bool CanReloadBots(object? arg) => !IsBotRunning;

    private bool CanEnableBotConnectionButton(object? arg) => IsSBoxConnected && SelectedBot != null;

    private void UpdateBotConnection(object? obj)
    {
        SelectedBot?.ToggleConnection();
        OnPropertyChanged(nameof(IsBotRunning));
    }

    private async void UpdateSBoxConnection(object? obj)
    {
        try
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await _sBoxClient.ToggleConnectionAsync();
        }
        finally
        {
            Mouse.OverrideCursor = null;
            OnPropertyChanged(nameof(IsSBoxConnected));
        }
    }

    private void ReloadBots(object? obj)
    {
        try
        {
            Mouse.OverrideCursor = Cursors.Wait;
            _botLoaderService.ReloadBots();
            LoadBotsToUI();
        }
        finally
        {
            Mouse.OverrideCursor = null;
        }
    }

    private void LoadBotsToUI()
    {
        AvailableBots.Clear();
        foreach (IBot bot in _botLoaderService.LoadedBots)
        {
            AvailableBots.Add(bot);
        }

        if (SelectedBot is null || !AvailableBots.Contains(SelectedBot))
        {
            SelectedBot = AvailableBots.FirstOrDefault();
        }
    }
}