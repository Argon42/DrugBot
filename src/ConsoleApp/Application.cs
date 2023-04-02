using DrugBot.Bot.Vk;
using Microsoft.Extensions.Logging;

namespace DrugBotApp;

public class Application : IApplication
{
    private readonly ILogger<Application> _logger;
    private readonly IVkBotHandler _vkBotHandler;

    public Application(ILogger<Application> logger, IVkBotHandler vkBotHandler)
    {
        _logger = logger;
        _vkBotHandler = vkBotHandler;
    }

    public void Run()
    {
        TryStartVk();
        StartTelegram();

        _logger.LogInformation("Application started");
        while (true)
        {
            string? input = Console.ReadLine();
            if (input == "quit")
            {
                _vkBotHandler.Dispose();
                return;
            }
        }
    }

    private void StartTelegram()
    {
        _logger.LogInformation("TelegramPlaceholder");
    }

    private async Task<bool> TryStartVk()
    {
        try
        {
            _vkBotHandler.Initialize();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed attempt to initialize vk");
            return false;
        }

        Task task = _vkBotHandler.Start();
        return true;
    }
}