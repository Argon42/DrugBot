using DrugBot.Core.Bot;

namespace DrugBotApp;

public interface IApplicationInput
{
    void Start(IEnumerable<IBotHandler> botHandlers);
}