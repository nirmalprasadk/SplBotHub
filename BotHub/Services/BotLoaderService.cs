using Microsoft.Extensions.DependencyInjection;
using Reusables.Contracts;
using System.IO;
using System.Reflection;

namespace BotHub.Services;

public class BotLoaderService : IBotLoaderService
{
    private readonly string _botFolder;
    private readonly List<IBot> _loadedBots;
    private readonly IServiceProvider _serviceProvider;

    public IReadOnlyList<IBot> LoadedBots => _loadedBots;

    public BotLoaderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _botFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bots");

        if (!Directory.Exists(_botFolder))
        {
            Directory.CreateDirectory(_botFolder);
        }

        _loadedBots = new List<IBot>();
        LoadBots();
    }

    public void ReloadBots()
    {
        StopAllBots();
        LoadBots();
    }

    public void StartAllBots() => _loadedBots.ForEach(bot => bot.Start());

    public void StopAllBots() => _loadedBots.ForEach(bot => bot.Stop());

    public void StartBot(IBot bot) => bot.Start();

    public void StopBot(IBot bot) => bot.Stop();

    private void LoadBots()
    {
        _loadedBots.Clear();

        foreach (string dll in Directory.GetFiles(_botFolder, "*.dll"))
        {
            try
            {
                foreach (Type type in loadBotTypesFromDll(dll))
                {
                    if (ActivatorUtilities.CreateInstance(_serviceProvider, type) is IBot bot)
                    {
                        _loadedBots.Add(bot);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }

    private static IEnumerable<Type> loadBotTypesFromDll(string dll)
    {
        return Assembly.LoadFrom(dll)
            .GetTypes()
            .Where(type => typeof(IBot).IsAssignableFrom(type)
            && !type.IsAbstract
            && type.IsClass);
    }
}