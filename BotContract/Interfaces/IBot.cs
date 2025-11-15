namespace BotContract.Interfaces;

public interface IBot
{
    string Name { get; }

    void Start();

    void Stop();
}