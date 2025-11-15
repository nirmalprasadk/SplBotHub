namespace Reusables.Contracts;

public interface IBot
{
    string Name { get; }

    void Start();

    void Stop();

    void ToggleConnection();

    bool IsRunning { get; }
}