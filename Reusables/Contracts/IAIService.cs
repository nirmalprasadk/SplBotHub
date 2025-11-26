namespace Reusables.Contracts;

public interface IAIService
{
    Task<string> AskAsync(string prompt, CancellationTokenSource cancellationToken);
}