using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DrugBot.Vk.Bot;

public class VkBotConfiguration
{
    private const string ResetErrorCounterDeltaTime = "VkBot.ResetErrorCounter.Seconds";
    private const string ResetErrorCounterErrors = "VkBot.ResetErrorCounter.ErrorCount";
    private const int DefaultErrorCount = 20;
    private const int DefaultSeconds = 10;
    private const string AppId = "VK_APP_ID";
    private const string GroupToken = "VK_GROUP_TOKEN";

    private readonly ILogger<VkBotConfiguration> _logger;
    private readonly IConfiguration _configuration;
    private TimeSpan _errorDeltaTime;
    private int _maxError;

    private string _groupToken;
    private uint _appId;

    public TimeSpan ErrorDeltaTime => _errorDeltaTime;
    public int MaxError => _maxError;
    public string Token => _groupToken;
    public uint Id => _appId;

    public VkBotConfiguration(ILogger<VkBotConfiguration> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _groupToken = "";
    }

    public void Initialize()
    {
        int seconds = _configuration.GetValue(ResetErrorCounterDeltaTime, DefaultSeconds);
        _errorDeltaTime = TimeSpan.FromSeconds(seconds);
        _maxError = _configuration.GetValue(ResetErrorCounterErrors, DefaultErrorCount);

        if (TryConfigureAuth(out _groupToken, out _appId) == false)
            throw new ConfigurationErrorsException($"{nameof(GroupToken)} or {AppId} incorrect");
    }

    private bool TryConfigureAuth(out string groupToken, out uint appId)
    {
        bool result = true;
        string? token = _configuration.GetValue<string?>(GroupToken, null);
        uint? id = _configuration.GetValue<uint?>(AppId, null);

        if (token == null)
        {
            _logger.LogInformation($"Environment variables {GroupToken} is incorrect");
            _logger.LogDebug("Token: {Token}", _groupToken);
            result = false;
        }

        if (id == null)
        {
            _logger.LogInformation($"Environment variable {AppId} is incorrect");
            _logger.LogDebug("id: {Id}", _configuration[AppId]);
            result = false;
        }

        groupToken = token ?? "";
        appId = id ?? 0;
        return result;
    }
}