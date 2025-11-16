namespace Reusables.Models;

public class AppConfig
{
    public SBoxServerConfig SBOXServerConfig { get; set; }

    public AIConfig AIConfig { get; set; }

    public AppConfig()
    {
        SBOXServerConfig = new SBoxServerConfig();
        AIConfig = new AIConfig();
    }
}