using System.Collections.ObjectModel;
using System.Text;
using DrugBot.Core.Bot;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace DrugBot.Vk.Bot;

public class VkBot : IBot<IVkUser, IVkMessage>, IBotHandler
{
    private readonly ILogger<VkBot> _logger;
    private readonly BotHandler _botHandler;
    private readonly IVkApi _api;

    private DateTime? _lastData;
    private string? _currentTs;
    private CancellationTokenSource? _cts;

    private int _errors;
    private DateTime _lastError = DateTime.MinValue;
    private readonly VkBotConfiguration _config;

    public bool IsWork { get; private set; }
    public string Name => nameof(VkBot);
    private static HttpClient Client { get; } = new();
    private bool IsInitialized { get; set; }

    public VkBot(
        ILogger<VkBot> logger,
        BotHandler botHandler,
        VkBotConfiguration configuration,
        IVkApi api)
    {
        _logger = logger;
        _botHandler = botHandler;
        _config = configuration;
        _api = api;
    }

    public void Initialize()
    {
        IsInitialized = false;
        _config.Initialize();

        if (_api.IsAuthorized)
            _api.LogOut();

        _api.Authorize(new ApiAuthParams { AccessToken = _config.Token });
        IsInitialized = true;
    }

    public long SendMessage(IVkMessage message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        if (IsWork == false || _api == null)
            throw new InvalidOperationException("Bot not working, messages unavailable");

        var messageId = _api.Messages.Send(new MessagesSendParams
        {
            PeerId = message.User.PeerId,
            Message = message.Text,
            RandomId = new Random().Next(),
            Forward = _config.NeedForward
                ? new MessageForward
                {
                    IsReply = true,
                    PeerId = message.User.PeerId,
                    ConversationMessageIds = message.ConversationMessageId != null
                        ? new[] { message.ConversationMessageId ?? 0 }
                        : null,
                }
                : default,
            Attachments = CreateMedia(message),
        });

        return messageId;
    }

    public long EditMessage(long messageId, IVkMessage message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        if (IsWork == false || _api == null)
            throw new InvalidOperationException("Bot not working, messages unavailable");

        var isEdited = _api.Messages.Edit(new MessageEditParams
        {
            PeerId = message.User.PeerId ?? 0,
            Message = message.Text,
            MessageId = messageId,
            KeepForwardMessages = _config.NeedForward,
            Attachments = CreateMedia(message),
        });

        if (!isEdited)
        {
            _logger.LogError($"Can't edit message {messageId} from {message.User.PeerId}");
        }

        return messageId;
    }

    public void Start()
    {
        if (IsInitialized == false)
            throw new InvalidOperationException("Bot not initialized");

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

    private async void BootLoop(object? o)
    {
        if (IsWork)
            throw new InvalidOperationException("Bot already working");
        if (_api == null || IsInitialized == false)
            throw new InvalidOperationException("Bot not initialized");

        CancellationToken token = (CancellationToken)(o ?? throw new ArgumentNullException(nameof(o)));

        try
        {
            IsWork = true;
            LongPollServerResponse longPollServer = CreateLongPool(_api);
            _currentTs = longPollServer.Ts;
            while (IsWork)
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    if (Update(longPollServer, _botHandler, token))
                        continue;
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (LongPollException exception)
                {
                    longPollServer = TryUpdateLongPollServer(exception, longPollServer, _api);
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
    }

    private IEnumerable<MediaAttachment> CreateMedia(IVkMessage vkMessage)
    {
        foreach (byte[] messageImage in vkMessage.Images)
        {
            string response = UploadPhoto(_api, messageImage);
            ReadOnlyCollection<Photo>? messagesPhoto = _api.Photo.SaveMessagesPhoto(response);
            if (messagesPhoto.FirstOrDefault() != null)
                yield return messagesPhoto.First();
        }
    }

    private void ErrorCounting()
    {
        if (DateTime.Now - _lastError > _config.ErrorDeltaTime)
        {
            _errors = 0;
            _lastError = DateTime.Now;
        }

        _errors++;

        if (_errors > _config.MaxError)
            throw new Exception($"From {_lastError} to {DateTime.Now} bot handled {_errors}. Stop working");
    }

    private LongPollServerResponse TryUpdateLongPollServer(
        LongPollException exception,
        LongPollServerResponse s, IVkApi api)
    {
        try
        {
            if (exception is LongPollOutdateException outdatedException)
                s.Ts = outdatedException.Ts;
            else
                s = CreateLongPool(api);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "can't find LongPoolServer");

            s = CreateLongPool(api);
        }

        return s;
    }

    private bool Update(
        LongPollServerResponse longPollServer,
        BotHandler botHandler,
        CancellationToken token)
    {
        BotsLongPollHistoryResponse? poll = _api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
        {
            Server = longPollServer.Server,
            Ts = _currentTs,
            Key = longPollServer.Key,
            Wait = 1000,
        });
        _currentTs = poll.Ts;

        if (poll.Updates == null || !poll.Updates.Any())
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
                .ContinueWith(task => { _logger.LogError(task.Exception, "Bot handler message processing was wrong"); },
                    TaskContinuationOptions.OnlyOnFaulted);
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

    private LongPollServerResponse CreateLongPool(IVkApi vkApi) => vkApi.Groups.GetLongPollServer(_config.Id);


    public IEnumerable<IVkUser> GetConversationMembers(IVkMessage message)
    {
        var result = _api.Messages
            .GetConversationMembers(message.User.PeerId.Value).Profiles.Select(x =>
                new VkUser(x.Id, message.User.PeerId, x.Status, x.FirstName, x.LastName));

        return result;
    }

    public IVkUser? GetUser(string userName)
    {
        var users = _api.Users.Get(new []{userName});

        if (!users.Any())
        {
            return null;
        }

        var user = users[0];
        var vkUser = new VkUser(user.Id, null, user.Status, user.FirstName, user.LastName);

        return vkUser;
    }
}