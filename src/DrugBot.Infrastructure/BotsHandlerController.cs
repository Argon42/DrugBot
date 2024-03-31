using DrugBot.Core.Bot;
using DrugBotApp;

public class BotsHandlerController : IBotsHandlerController
{
    private readonly IBotHandler[] _botHandlers;
    public bool IsInitialized { get; private set; }

    public BotsHandlerController(IEnumerable<IBotHandler> botHandlers)
    {
        _botHandlers = botHandlers.ToArray();
    }

    public void Initialize()
    {
        IsInitialized = true;
    }

    public void RestartHandler(string handlerName)
    {
        ValidateState();

        StopHandler(handlerName);
        StartHandler(handlerName);
    }

    public void StopHandler(string handlerName)
    {
        ValidateState();

        IBotHandler botHandler = _botHandlers.First(handler => handler.Name == handlerName);
        botHandler.Stop();
    }

    private void ValidateState()
    {
        if (IsInitialized == false)
            throw new InvalidOperationException("Application is not working");
    }

    public void StartHandler(string handlerName)
    {
        ValidateState();
        IBotHandler botHandler = _botHandlers.First(handler => handler.Name == handlerName);
        botHandler.Start();
    }

    public IEnumerable<BotStatus> GetStatus() => 
        _botHandlers.Select(handler => new BotStatus(handler.Name, handler.IsWork));
}