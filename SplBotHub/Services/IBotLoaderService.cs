using SplBotHub.Bots;

namespace SplBotHub.Services;

public interface IBotLoaderService
{
    IReadOnlyList<IBot> LoadedBots { get; }

    void ReloadBots();
}