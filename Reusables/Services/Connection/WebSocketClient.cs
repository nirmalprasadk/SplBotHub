using Reusables.Contracts;
using System.Net.WebSockets;
using System.Text;

namespace BotHub.Services.Connection;

public class WebSocketClient : IClient
{
    private ClientWebSocket _webSocket;
    private CancellationTokenSource? _receiveLoopCancellationToken;

    public event Action<string>? OnMessageReceived;

    public WebSocketClient()
    {
        _webSocket = new ClientWebSocket();
    }

    public async Task ConnectAsync(Uri serverUri, CancellationToken cancellationToken = default)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            await DisconnectAsync(cancellationToken);
        }

        await _webSocket.ConnectAsync(serverUri, cancellationToken);

        BeginMessageProcessing();
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if(_webSocket.State != WebSocketState.Open)
        {
            return;
        }

        _receiveLoopCancellationToken?.Cancel();
        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            throw new InvalidOperationException("WebSocket is not connected.");
        }

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
        ArraySegment<byte> segment = new(bytes);

        await _webSocket.SendAsync(
            segment,
            WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken);
    }

    private void BeginMessageProcessing()
    {
        _receiveLoopCancellationToken = new CancellationTokenSource();
        _ = Task.Run(() => ReceiveLoop(_receiveLoopCancellationToken.Token));
    }

    private async Task ReceiveLoop(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[4096];

        try
        {
            while (!cancellationToken.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> messageBuffer = new(buffer);
                StringBuilder builder = new();
                WebSocketReceiveResult? result;

                do
                {
                    result = await _webSocket.ReceiveAsync(messageBuffer, cancellationToken);
                    string chunk = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    builder.Append(chunk);
                } 
                while (!result.EndOfMessage);

                string message = builder.ToString();
                OnMessageReceived?.Invoke(message);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket receive error: {ex.Message}");
        }
        finally
        {
            await DisconnectAsync();
        }
    }
}