using System.Collections;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using Microsoft.Extensions.Logging;

namespace DrugBotApp;

public class Application : IApplication
{
    private readonly ILogger<Application> _logger;
    private readonly IEnumerable<IBotHandler> _bots;
    private readonly IApplicationInput _input;

    public Application(ILogger<Application> logger, IEnumerable<IBotHandler> bots, IApplicationInput input)
    {
        _logger = logger;
        _bots = bots;
        _input = input;
    }

    public void Run()
    {
        Initialize();
        Start();
        HealthCheck();
    }

    private void HealthCheck()
    {
        // TODO: добавить отображение текущего состояния ботов
        while (true)
        {
            Task.Delay(100);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private void Initialize()
    {
        _logger.LogInformation("Bots initializing");
        _bots.ForEach(InitializeHandler);
        _logger.LogInformation("Bots initialized");
    }

    private void Start()
    {
        _bots.ForEach(StartHandlers);
        _input.Start(_bots);
        _logger.LogInformation("Application started");
    }

    private void InitializeHandler(IBotHandler handler)
    {
        try
        {
            handler.Initialize();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Initialize handlers");
        }
    }

    private void StartHandlers(IBotHandler handler)
    {
        try
        {
            handler.Start();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Initialize handlers");
        }
    }
}