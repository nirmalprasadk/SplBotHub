using BotContract.Interfaces;
using BotHub.Services;
using BotHub.Services.Connection;
using System.Collections.ObjectModel;

namespace BotHub.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private bool _isConnectionEstablishedWithSBox;
    private bool _isBotConnected;
    private readonly ISboxConnectionService _sboxConnectionService;
    private readonly IBotLoaderService _botLoaderService;

    public string WindowTitle => "SPL Bot Hub";

    public bool IsConnectionEstablishedWithSBox
    {
        get => _isConnectionEstablishedWithSBox; 
        set
        {
            _isConnectionEstablishedWithSBox = value;
            OnPropertyChanged();
        }
    }

    public BindingCommand SBoxConnectionCommand { get; private set; }

    public BindingCommand ReloadBotsCommand { get; private set; }

    public BindingCommand BotConnectionCommand { get; private set; }

    public bool IsBotRunning => SelectedBot is not null && SelectedBot.IsRunning;

    public ObservableCollection<IBot> AvailableBots { get; }

    public IBot? SelectedBot { get; set; }

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

    private void UpdateBotConnection(object? obj) => SelectedBot?.ToggleConnection();

    private async void UpdateSBoxConnection(object? obj)
    {
        try
        {
            await _sboxConnectionService.ToggleConnection(IsConnectionEstablishedWithSBox);
            IsConnectionEstablishedWithSBox = !IsConnectionEstablishedWithSBox;
        }
        catch (Exception)
        {
        }
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
    }
}