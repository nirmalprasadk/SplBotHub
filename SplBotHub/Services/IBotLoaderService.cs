using BotContract.Interfaces;

namespace SplBotHub.Services;

public interface IBotLoaderService
{
    IReadOnlyList<IBot> LoadedBots { get; }

    void ReloadBots();

    void StartAllBots();

    void StopAllBots();

    void StartBot(IBot bot);

    void StopBot(IBot bot);
}