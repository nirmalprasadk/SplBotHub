using System.Threading.Channels;

namespace Reusables.Contracts;

public interface IClient
{
    Task ConnectAsync(Uri serverUri, CancellationToken cancellationToken);

    Task DisconnectAsync(CancellationToken cancellationToken);

    Task SendMessageAsync(string message, CancellationToken cancellationToken);

    ChannelReader<string> Messages { get; }
}