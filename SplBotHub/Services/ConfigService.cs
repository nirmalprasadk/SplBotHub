namespace SplBotHub.Services;

using System.IO;
using System.Text.Json;

public static class ConfigService
{
    private const string ConfigPath = "appsettings.json";

    public static AppConfig? Load()
    {
        if (!File.Exists(ConfigPath))
        {
            AppConfig defaultConfig = new();
            Save(defaultConfig);
            return defaultConfig;
        }

        string json = File.ReadAllText(ConfigPath);
        return JsonSerializer.Deserialize<AppConfig>(json);
    }

    public static void Save(AppConfig config)
    {
        string json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(ConfigPath, json);
    }
}

