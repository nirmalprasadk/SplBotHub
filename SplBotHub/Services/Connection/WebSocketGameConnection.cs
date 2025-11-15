using BotContract.Interfaces;
using System.Net.WebSockets;

namespace SplBotHub.Services.Connection;

public class WebSocketGameConnection : IGameConnection
{
    private ClientWebSocket _webSocket;

    public WebSocketGameConnection()
    {
        _webSocket = new ClientWebSocket();
    }

    public async Task ConnectAsync(Uri serverUri, CancellationToken cancellationToken)
    {
        await _webSocket.ConnectAsync(serverUri, cancellationToken);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
    }
}