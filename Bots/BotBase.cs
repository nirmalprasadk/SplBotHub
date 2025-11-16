using Reusables.Contracts;

namespace Bots;

public abstract class BotBase : IBot
{
    protected readonly ISboxClient SBoxClient;
    protected readonly IAIService AIService;

    public string Name { get; }

    public bool IsRunning { get; private set; }

    protected BotBase(ISboxClient sBoxClient, IAIService aIService, string? name = null)
    {
        SBoxClient = sBoxClient;
        AIService = aIService;

        Name = name ?? GetType().Name;
    }

    public virtual void Start()
    {
        SBoxClient.MessageReceived += OnSBoxMessageReceivedInternal;
        IsRunning = true;
    }

    public virtual void Stop()
    {
        SBoxClient.MessageReceived -= OnSBoxMessageReceivedInternal;
        IsRunning = false;
    }

    public void ToggleConnection()
    {
        if (IsRunning)
        {
            Stop();
        }
        else
        {
           Start();
        }
    }

    protected async Task SendMessageToSBoxInternal(string message)
    {
        await SBoxClient.SendMessageAsync(message);
    }

    async Task IBot.SendMessageToSBox(string message)
    {
        await SendMessageToSBoxInternal(message);
    }

    private void OnSBoxMessageReceivedInternal(string message)
    {
        OnSBoxMessageReceived(message);
    }

    protected abstract void OnSBoxMessageReceived(string message);
}
