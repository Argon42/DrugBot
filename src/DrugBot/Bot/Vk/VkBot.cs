using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DrugBot.Core.Common;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace DrugBot.Bot.Vk;

public class VkBot : IVkBot
{
    private readonly ILogger<VkBot> _logger;
    private readonly BotHandler _botHandler;
    private readonly IFactory<IVkApi> _vkApiFactory;
    private readonly IFactory<LongPollServerResponse> _longPoolServerFactory;
    private DateTime? _lastData;
    private IVkApi? _api;
    private string? _currentTs;

    private static HttpClient Client { get; } = new();

    public VkBot(ILogger<VkBot> logger, BotHandler botHandler, IFactory<IVkApi> vkApiFactory,
        IFactory<LongPollServerResponse> longPoolServerFactory)
    {
        _logger = logger;
        _botHandler = botHandler;
        _vkApiFactory = vkApiFactory;
        _longPoolServerFactory = longPoolServerFactory;
    }


    static VkBot()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public void SendMessage(IVkMessage message)
    {
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

    public async Task Start(CancellationToken token)
    {
        _api = _vkApiFactory.Create();
        LongPollServerResponse longPollServer = _longPoolServerFactory.Create();
        _currentTs = longPollServer.Ts;
        while (token.IsCancellationRequested == false)
        {
            try
            {
                if (Update(longPollServer, _botHandler, token)) continue;
            }
            catch (LongPollException exception)
            {
                longPollServer = TryUpdateLongPollServer(exception, longPollServer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Server update data");
            }

            await Task.Delay(100, token);
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

    private bool Update(LongPollServerResponse longPollServer, BotHandler botHandler, CancellationToken token)
    {
        BotsLongPollHistoryResponse? poll = _api!.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
        {
            Server = longPollServer.Server,
            Ts = _currentTs,
            Key = longPollServer.Key,
            Wait = 1000,
        });

        if (poll?.Updates == null || !poll.Updates.Any()) return true;

        foreach (GroupUpdate? a in poll.Updates)
        {
            _currentTs = poll.Ts;
            Message? message = a.Instance as Message ?? (a.Instance as MessageNew)?.Message;
            if (message == null) continue;

            if (message.Date > _lastData || _lastData == null)
            {
                _lastData = message.Date;

                botHandler.MessageProcessing(new VkMessage(message), this, token);
            }
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