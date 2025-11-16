using Reusables.Enums;
using System.Text.Json.Serialization;

namespace Reusables.Models;

public class AIConfig
{
    public string? ApiKey { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OpenAIModel DefaultModel { get; set; }
}