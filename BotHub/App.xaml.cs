using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BotHub.ViewModels;
using System.Windows;
using Reusables.Contracts;
using BotHub.Services.Connection;
using Reusables.Services.Connection;
using BotHub.Services;
using Reusables.Services.AI;

namespace BotHub;

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
        services.AddSingleton<IClient, WebSocketClient>();
        services.AddSingleton<ISboxClient, SBoxConnectionService>();
        services.AddSingleton<IBotLoaderService, BotLoaderService>();
        services.AddSingleton<IAIService, OpenAIService>();
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