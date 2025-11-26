using Reusables.Contracts;
using Reusables.Enums;
using Reusables.Models.SBoxMessage;
using Reusables.Parsers;

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

    private void OnSBoxMessageReceivedInternal(string message)
    {
        if (SBoxMessageParser.Parse(message) is not SBoxMessageBase sBoxMessage)
        {
            return;
        }

        OnSBoxMessageReceived(sBoxMessage);
    }

    protected virtual void OnSBoxMessageReceived(SBoxMessageBase sBoxMessage)
    {
        switch (sBoxMessage)
        {
            case AckMessage ack:
                HandleAck(ack);
                break;

            case CommandMessage cmd:
                HandleCommand(cmd);
                break;

            case GameResultMessage result:
                HandleGameResult(result);
                break;
        }
    }

    protected abstract void HandleAck(AckMessage ack);

    protected abstract void HandleCommand(CommandMessage command);

    protected abstract void HandleGameResult(GameResultMessage gameResult);

    protected async Task SendMessageToSBoxInternal(string message)
    {
        await SBoxClient.SendMessageAsync(message);
    }

    protected void Log(string message)
    {
        SBoxClient.Log(MessageSource.Bot, message);
    }

    async Task IBot.SendMessageToSBox(BotGuessMessage response)
    {
        string responseJson = SBoxMessageParser.Serialize(response);
        await SendMessageToSBoxInternal(responseJson);
    }
}
