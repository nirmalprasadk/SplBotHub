using Reusables.Models;
using System.Text.Json;

namespace Reusables.Services;

public static class ConfigService
{
    private const string ConfigPath = "appsettings.json";

    public static AppConfig Load()
    {
        if (!File.Exists(ConfigPath))
        {
            AppConfig defaultConfig = new()
            {
                SBOXServerConfig = new()
            };

            Save(defaultConfig);
            return defaultConfig;
        }

        string json = File.ReadAllText(ConfigPath);

        return JsonSerializer.Deserialize<AppConfig>(json)
            ?? new AppConfig { SBOXServerConfig = new() };
    }

    public static void Save(AppConfig config)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(config, options);

        File.WriteAllText(ConfigPath, json);
    }
}
