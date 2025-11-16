using Reusables.Enums;

namespace Reusables.Models;

public class AIConfig
{
    public string? ApiKey { get; set; }

    public OpenAIModel DefaultModel { get; set; }
}