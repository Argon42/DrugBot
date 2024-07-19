using DrugBot.Core;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DrugBot.Infrastructure;

public class Application : BackgroundService
{
    private readonly ILogger<Application> _logger;
    private readonly IEnumerable<IBotHandler> _bots;
    private readonly IBotsHandlerController _input;

    public Application(ILogger<Application> logger, IEnumerable<IBotHandler> bots, IBotsHandlerController input)
    {
        _logger = logger;
        _bots = bots;
        _input = input;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Run();
        return Task.CompletedTask;
    }

    private void Run()
    {
        Initialize();
        Start();
        HealthCheck();
    }

    private async void HealthCheck()
    {
        // TODO: добавить отображение текущего состояния ботов
        while (true)
        {
            await Task.Delay(100);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private void Initialize()
    {
        _logger.LogInformation("Bots initializing");
        _bots.ForEach(InitializeHandler);
        _logger.LogInformation("Bots initialized");
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

    private void Start()
    {
        _bots.ForEach(StartHandlers);
        _input.Initialize();
        _logger.LogInformation("Application started");
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