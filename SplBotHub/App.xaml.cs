using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SplBotHub.Connection;
using SplBotHub.Services;
using SplBotHub.ViewModels;
using System.Windows;

namespace SplBotHub;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static  IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
        {
            RegisterServices(services);
            RegisterViewModels(services);
            RegisterViews(services);

        }).Build();
    }

    private void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IGameConnection, WebSocketGameConnection>();
        services.AddSingleton<ISboxConnectionService, SBoxConnectionService>();
        services.AddSingleton<IBotLoaderService, BotLoaderService>();
    }

    private void RegisterViewModels(IServiceCollection services)
    {
       services.AddSingleton<MainWindowViewModel>();
    }

    private void RegisterViews(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        AppHost?.Start();

        MainWindow? mainWindow = AppHost?.Services.GetRequiredService<MainWindow>();
        mainWindow?.Show();

        base.OnStartup(args);
    }
}