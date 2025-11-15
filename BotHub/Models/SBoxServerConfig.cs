namespace BotHub.Models;

public class SBoxServerConfig
{
    public string Host { get; set; } = "localhost";

    public int Port { get; set; } = 8080;

    public bool UseSecureConnection { get; set; } = false;

    public Uri BuildUri()
    {
        string scheme = UseSecureConnection ? "wss" : "ws";
        return new Uri($"{scheme}://{Host}:{Port}");
    }
}