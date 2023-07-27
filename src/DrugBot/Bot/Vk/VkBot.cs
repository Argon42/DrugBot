using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace DrugBot.Bot.Vk;

public class VkBot : IVkBot, IBotHandler
{
    private const string ResetErrorCounterDeltaTime = "VkBot.ResetErrorCounter.Seconds";
    private const string ResetErrorCounterErrors = "VkBot.ResetErrorCounter.ErrorCount";
    private const int DefaultErrorCount = 20;
    private const int DefaultSeconds = 10;

    private readonly ILogger<VkBot> _logger;
    private readonly BotHandler _botHandler;
    private readonly IFactory<IVkApi> _vkApiFactory;
    private readonly IFactory<LongPollServerResponse> _longPoolServerFactory;
    private readonly TimeSpan _resetErrorCounterDeltaTime;
    private readonly int _maxErrorsPerDeltaTime;

    private DateTime? _lastData;
    private IVkApi? _api;
    private string? _currentTs;
    private CancellationTokenSource? _cts;

    private int _errors = 0;
    private DateTime _lastError = DateTime.MinValue;

    public bool IsWork { get; private set; }
    public string Name => nameof(VkBot);
    private static HttpClient Client { get; } = new();


    public VkBot(
        ILogger<VkBot> logger,
        BotHandler botHandler,
        IFactory<IVkApi> vkApiFactory,
        IFactory<LongPollServerResponse> longPoolServerFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _botHandler = botHandler;
        _vkApiFactory = vkApiFactory;
        _longPoolServerFactory = longPoolServerFactory;
        
        _resetErrorCounterDeltaTime = TimeSpan.FromSeconds(
            int.TryParse(configuration[ResetErrorCounterDeltaTime], out int seconds) ? seconds : DefaultSeconds);
        
        _maxErrorsPerDeltaTime = 
            int.TryParse(configuration[ResetErrorCounterErrors], out int errorCount) ? errorCount : DefaultErrorCount;
    }

    public void Initialize()
    {
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        Thread thread = new(BootLoop);
        thread.Start(_cts.Token);
    }

    public void Stop()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    public void SendMessage(IVkMessage message)
    {
        if (IsWork == false || _api == null)
        {
            throw new InvalidOperationException("Bot not working, messages unavailable");
        }

        bool needForward = false;
        _api.Messages.Send(new MessagesSendParams
        {
            PeerId = message.User.PeerId,
            Message = message.Text,
            RandomId = new Random().Next(),
            ReplyTo = needForward ? message.TriggerMessage.ConversationMessageId.GetValueOrDefault() : default,
            Attachments = CreateMedia(message),
        });
    }

    private async void BootLoop(object? o)
    {
        if (IsWork)
        {
            throw new InvalidOperationException("Bot already working");
        }
        CancellationToken token = (CancellationToken)(o ?? throw new ArgumentNullException(nameof(o)));
        try
        {
            IsWork = true;
            _api = _vkApiFactory.Create();
            LongPollServerResponse longPollServer = _longPoolServerFactory.Create();
            _currentTs = longPollServer.Ts;
            while (IsWork)
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    if(Update(longPollServer, _botHandler, token))
                        continue;
                }
                catch (LongPollException exception)
                {
                    longPollServer = TryUpdateLongPollServer(exception, longPollServer);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Server update data");
                    ErrorCounting();
                }

                await Task.Delay(100, token);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Bot disabled by token");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Bot disabled");
        }
        finally
        {
            IsWork = false;
        }
        token.ThrowIfCancellationRequested();
    }

    private void ErrorCounting()
    {
        if (DateTime.Now - _lastError > _resetErrorCounterDeltaTime)
        {
            _errors = 0;
            _lastError = DateTime.Now;
        }
        _errors++;
        
        if (_errors > _maxErrorsPerDeltaTime)
        {
            throw new Exception($"From {_lastError} to {DateTime.Now} bot handled {_errors}. Stop working");
        }
    }

    private IEnumerable<MediaAttachment> CreateMedia(IVkMessage vkMessage)
    {
        foreach (byte[] messageImage in vkMessage.Images)
        {
            string response = UploadPhoto(_api!, messageImage);
            ReadOnlyCollection<Photo>? messagesPhoto = _api!.Photo.SaveMessagesPhoto(response);
            if (messagesPhoto.FirstOrDefault() != null)
                yield return messagesPhoto.First();
        }
    }

    private LongPollServerResponse TryUpdateLongPollServer(
        LongPollException exception,
        LongPollServerResponse s)
    {
        try
        {
            if (exception is LongPollOutdateException outdateException)
                s.Ts = outdateException.Ts;
            else
                s = _longPoolServerFactory.Create();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "can't find LongPoolServer");

            s = _longPoolServerFactory.Create();
        }

        return s;
    }

    private bool Update(
        LongPollServerResponse longPollServer, 
        BotHandler botHandler,
        CancellationToken token)
    {
        BotsLongPollHistoryResponse? poll = _api!.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
        {
            Server = longPollServer.Server,
            Ts = _currentTs,
            Key = longPollServer.Key,
            Wait = 1000,
        });
        _currentTs = poll.Ts;

        if (poll?.Updates == null || !poll.Updates.Any()) 
            return true;

        foreach (GroupUpdate? a in poll.Updates)
        {
            _currentTs = poll.Ts;
            Message? message = a.Instance as Message ?? (a.Instance as MessageNew)?.Message;
            if (message == null) 
                continue;

            if (message.Date <= _lastData && _lastData != null) 
                continue;
            
            _lastData = message.Date;

            botHandler.MessageProcessing(new VkMessage(message), this, token)
                .ContinueWith(task =>
                {
                    _logger.LogError(task.Exception, "Bot handler message processing was wrong");
                }, TaskContinuationOptions.OnlyOnFaulted);
        }

        return false;
    }

    private static string UploadPhoto(IVkApi api, byte[] image)
    {
        MultipartFormDataContent content = new();
        UploadServerInfo? uploadServer = api.Photo.GetMessagesUploadServer(api.UserId.GetValueOrDefault());
        content.Add(new ByteArrayContent(image), "file", "photo.jpg");
        HttpResponseMessage responseMessage =
            Client.PostAsync(uploadServer.UploadUrl, content).GetAwaiter().GetResult();
        byte[] responseRaw = responseMessage.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
        return Encoding.GetEncoding(1251).GetString(responseRaw);
    }
}