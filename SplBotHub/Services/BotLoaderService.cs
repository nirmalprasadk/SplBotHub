using SplBotHub.Bots;
using System.IO;
using System.Reflection;

namespace SplBotHub.Services;

public class BotLoaderService : IBotLoaderService
{
    private readonly string _botFolder;
    private readonly List<IBot> _loadedBots;

    public IReadOnlyList<IBot> LoadedBots => _loadedBots;

    public BotLoaderService()
    {
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
        LoadBots();
    }

    private void LoadBots()
    {
        _loadedBots.Clear();

        foreach (string dll in Directory.GetFiles(_botFolder, "*.dll"))
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dll);

                IEnumerable<IBot?> bots = assembly.GetTypes()
                    .Where(type => typeof(IBot).IsAssignableFrom(type) && !type.IsAbstract)
                    .Select(type => Activator.CreateInstance(type) as IBot)
                    .Where(bot => bot != null);

                _loadedBots.AddRange(bots!);
            }
            catch
            {
                // Log or ignore invalid DLLs
            }
        }
    }
}