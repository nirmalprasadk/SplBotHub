using Reusables.Contracts;
using System.Collections.ObjectModel;
using Reusables.Models;
using System.Windows.Input;
using Reusables.Enums;
using System.Collections.Specialized;

namespace BotHub.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ISboxClient _sBoxClient;
    private readonly IBotLoaderService _botLoaderService;
    private IBot? _selectedBot;
    private string? _selectedSourceFilter;
    private string? _selectedGameIdFilter;
    private const string NoneFilter = "None";

    public string WindowTitle => "SPL Bot Hub";

    public bool IsSBoxConnected => _sBoxClient.IsConnected;

    public BindingCommand SBoxConnectionCommand { get; private set; }

    public BindingCommand ReloadBotsCommand { get; private set; }

    public BindingCommand BotConnectionCommand { get; private set; }

    public BindingCommand ClearLogsCommand { get; private set; }

    public bool IsBotRunning => SelectedBot is not null && SelectedBot.IsRunning;

    public ObservableCollection<IBot> AvailableBots { get; }

    public IBot? SelectedBot
    {
        get => _selectedBot; 
        set
        {
            _selectedBot?.Stop();
            _selectedBot = value;
            _selectedBot?.Start();
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanDisplayLogs));
        }
    }

    public bool CanDisplayLogs => SelectedBot is not null;

    public ObservableCollection<BotLogEntry> FilteredSBoxLogs { get; }

    public List<string> SourceFilterOptions { get; private set; }

    public ObservableCollection<string> GameIdsInLogs { get; private set; }

    public string? SelectedSourceFilter
    {
        get => _selectedSourceFilter; 
        set
        {
            _selectedSourceFilter = value;
            OnPropertyChanged();
            RefreshFilteredLogs();
        }
    }

    public string? SelectedGameIdFilter
    {
        get => _selectedGameIdFilter; set
        {
            _selectedGameIdFilter = value;
            OnPropertyChanged();
            RefreshFilteredLogs();
        }
    }

    public MainWindowViewModel(ISboxClient sBoxClient, IBotLoaderService botLoaderService)
    {
        _sBoxClient = sBoxClient;
        _botLoaderService = botLoaderService;

        SBoxConnectionCommand = new BindingCommand(UpdateSBoxConnection);
        ReloadBotsCommand = new BindingCommand(ReloadBots, CanReloadBots);
        BotConnectionCommand = new BindingCommand(UpdateBotConnection, CanEnableBotConnectionButton);
        ClearLogsCommand = new BindingCommand(ClearSBoxLogs);

        FilteredSBoxLogs = new ObservableCollection<BotLogEntry>();

        SourceFilterOptions = SourceFilterOptions = Enum.GetNames(typeof(MessageSource)).ToList();
        SourceFilterOptions.Insert(0, NoneFilter);
        SelectedSourceFilter = NoneFilter;
        GameIdsInLogs = new ObservableCollection<string>();
        GameIdsInLogs.Insert(0, NoneFilter);
        SelectedGameIdFilter = NoneFilter;

        _sBoxClient.Logs.CollectionChanged += SBoxLogsChanged;

        AvailableBots = new ObservableCollection<IBot>();
        LoadBotsToUI();
    }

    public void OnWindowClosed(object? sender, EventArgs args)
    {
        _botLoaderService.StopAllBots();
        _sBoxClient.DisconnectAsync().GetAwaiter().GetResult();
    }

    private void SBoxLogsChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        if (args.Action == NotifyCollectionChangedAction.Reset)
        {
            GameIdsInLogs.Clear();
        }
        else
        {
            RefreshGameIds(args);
        }

        RefreshFilteredLogs();
    }


    private void RefreshGameIds(NotifyCollectionChangedEventArgs args)
    {
        if (args.Action != NotifyCollectionChangedAction.Add)
        {
            return;
        }

        foreach (object? item in args.NewItems!)
        {
            if (item is BotLogEntry log 
                && !string.IsNullOrWhiteSpace(log.GameId) 
                && !GameIdsInLogs.Contains(log.GameId))
            {
                GameIdsInLogs.Add(log.GameId);
            }
        }
    }

    private void RefreshFilteredLogs()
    {
        IEnumerable<BotLogEntry> logs = _sBoxClient.Logs;

        if (SelectedSourceFilter != null && SelectedSourceFilter != NoneFilter)
        {
            logs = logs.Where(log => log.MessageSource.ToString() == SelectedSourceFilter);
        }

        if (!string.IsNullOrWhiteSpace(SelectedGameIdFilter) && SelectedGameIdFilter != NoneFilter)
        {
            logs = logs.Where(log => log.GameId != null && log.GameId.Contains(SelectedGameIdFilter, StringComparison.OrdinalIgnoreCase));
        }

        FilteredSBoxLogs.Clear();
        foreach (BotLogEntry log in logs)
        {
            FilteredSBoxLogs.Add(log);
        }
    }

    private void ClearSBoxLogs(object? obj)
    {
        _sBoxClient.ClearLogs();
    }

    private bool CanReloadBots(object? arg) => !IsBotRunning;

    private bool CanEnableBotConnectionButton(object? arg) => SelectedBot != null;

    private void UpdateBotConnection(object? obj)
    {
        try
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedBot?.ToggleConnection();
        }
        finally
        {
            Mouse.OverrideCursor = null;
            OnPropertyChanged(nameof(IsBotRunning));
            ReloadBotsCommand.RaiseCanExecuteChanged();
        }
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