using Reusables.Models;
using System.Text.Json;

namespace Reusables.Services;

public static class ConfigService
{
    private const string ConfigPath = "appsettings.json";
    private static AppConfig? AppConfig;

    public static AppConfig GetAppConfig()
    {
        AppConfig ??= Load();
        return AppConfig;
    }

    private static AppConfig Load()
    {
        AppConfig defaultAppConfig = new();

        if (File.Exists(ConfigPath))
        {
            string json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? defaultAppConfig;
        }

        Save(defaultAppConfig);
        return defaultAppConfig;
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
