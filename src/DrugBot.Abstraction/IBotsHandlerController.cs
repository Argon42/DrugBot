using DrugBot.Core.Bot;

namespace DrugBot.Core;

public interface IBotsHandlerController
{
    void Initialize();
    void RestartHandler(string handlerName);
    void StopHandler(string handlerName);
    void StartHandler(string handlerName);
    IEnumerable<BotStatus> GetStatus();
    bool IsInitialized { get; }
}