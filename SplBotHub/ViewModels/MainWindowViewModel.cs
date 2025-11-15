using BotContract.Interfaces;
using SplBotHub.Services;
using SplBotHub.Services.Connection;
using System.Collections.ObjectModel;

namespace SplBotHub.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private bool _isConnectionEstablishedWithSBox;
    private readonly ISboxConnectionService _sboxConnectionService;
    private readonly IBotLoaderService _botLoaderService;

    public bool IsConnectionEstablishedWithSBox
    {
        get => _isConnectionEstablishedWithSBox; 
        set
        {
            _isConnectionEstablishedWithSBox = value;
            OnPropertyChanged();
        }
    }

    public BindingCommand SBoxConnectionCommand { get; }

    public BindingCommand ReloadBotsCommand { get; }

    public ObservableCollection<IBot> AvailableBots { get; }

    public IBot? SelectedBot { get; set; }

    public MainWindowViewModel(ISboxConnectionService sboxConnectionService, IBotLoaderService botLoaderService)
    {
        _sboxConnectionService = sboxConnectionService;
        _botLoaderService = botLoaderService;

        SBoxConnectionCommand = new BindingCommand(UpdateSBoxConnection);
        ReloadBotsCommand = new BindingCommand(ReloadBots);

        AvailableBots = new ObservableCollection<IBot>();
        LoadBotsToUI();
    }

    private async void UpdateSBoxConnection(object? obj)
    {
        try
        {
            await _sboxConnectionService.ToggleSBoxConnection(IsConnectionEstablishedWithSBox);
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