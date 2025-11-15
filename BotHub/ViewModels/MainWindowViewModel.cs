using Reusables.Services;
using Reusables.Contracts;
using Reusables.Services.Connection;
using System.Collections.ObjectModel;

namespace BotHub.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ISboxConnectionService _sboxConnectionService;
    private readonly IBotLoaderService _botLoaderService;
    private IBot? _selectedBot;

    public string WindowTitle => "SPL Bot Hub";

    public bool IsSBoxConnected => _sboxConnectionService.IsConnected;

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
        }
    }

    public MainWindowViewModel(ISboxConnectionService sboxConnectionService, IBotLoaderService botLoaderService)
    {
        _sboxConnectionService = sboxConnectionService;
        _botLoaderService = botLoaderService;

        SBoxConnectionCommand = new BindingCommand(UpdateSBoxConnection);
        ReloadBotsCommand = new BindingCommand(ReloadBots);
        BotConnectionCommand = new BindingCommand(UpdateBotConnection, CanEnableBotConnectionButton);

        AvailableBots = new ObservableCollection<IBot>();
        LoadBotsToUI();
    }

    private bool CanEnableBotConnectionButton(object? arg) => SelectedBot != null;

    private void UpdateBotConnection(object? obj)
    {
        SelectedBot?.ToggleConnection();
        OnPropertyChanged(nameof(IsBotRunning));
    }

    private async void UpdateSBoxConnection(object? obj)
    {
        await _sboxConnectionService.ToggleConnection();
        OnPropertyChanged(nameof(IsSBoxConnected));
    }

    private void ReloadBots(object? obj)
    {
        _botLoaderService.ReloadBots();
        LoadBotsToUI();
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