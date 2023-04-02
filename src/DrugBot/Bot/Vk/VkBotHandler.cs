using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DrugBot.Bot.Vk;

public class VkBotHandler : IVkBotHandler
{
    private readonly ILogger<VkBotHandler> _logger;
    private readonly IConfiguration _configuration;
    private CancellationTokenSource? _tokenSource;
    private readonly IVkBot _vkBot;

    public bool Enabled { get; private set; }

    public VkBotHandler(ILogger<VkBotHandler> logger, IConfiguration configuration, IVkBot vkBot)
    {
        _logger = logger;
        _configuration = configuration;
        _vkBot = vkBot;
    }

    public void Dispose()
    {
        Stop();
    }

    public void Initialize()
    {
    }

    public async Task Start()
    {
        if (Enabled)
            _tokenSource!.Cancel();

        _tokenSource = new CancellationTokenSource();
        Enabled = true;
        _vkBot.Start(_tokenSource.Token);
    }

    public void Stop()
    {
        _tokenSource?.Cancel();
        Enabled = false;
    }
}