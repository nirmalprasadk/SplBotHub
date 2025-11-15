namespace SplBotHub.Models;

public class GameConnectionConfig
{
    public string ServerHost { get; set; } = "localhost";
    public int ServerPort { get; set; } = 8080;
    public bool UseSecureConnection { get; set; } = false;

    public Uri BuildUri()
    {
        string scheme = UseSecureConnection ? "wss" : "ws";
        return new Uri($"{scheme}://{ServerHost}:{ServerPort}");
    }
}