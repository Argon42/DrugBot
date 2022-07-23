using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BananvaBot;
using VkNet;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace DrugBot
{
    public class Bot
    {
        private DateTime? _lastData;
        private VkApi _api = new VkApi();
        private string _currentTs;


        public async Task Start(CancellationToken token)
        {
            var configs = Configs.GetConfig();
            var longPollServer = Auth(configs);
            var botHandler = new BotHandler(_api);
            _currentTs = longPollServer.Ts;
            while (token.IsCancellationRequested == false)
            {
                try
                {
                    var poll = _api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                    {
                        Server = longPollServer.Server,
                        Ts = _currentTs,
                        Key = longPollServer.Key,
                        Wait = 130
                    });

                    if (poll?.Updates == null || !poll.Updates.Any()) continue;

                    foreach (var a in poll.Updates)
                    {
                        _currentTs = poll.Ts;

                        Message message;
                        if (a.MessageNew != null)
                            message = a.MessageNew.Message;
                        else if (a.Message != null)
                            message = a.Message;
                        else
                            continue;

                        if (message.Date > _lastData || _lastData == null)
                        {
                            _lastData = message.Date;
                            botHandler.MessageProcessing(message);
                        }
                    }
                }
                catch (LongPollException exception)
                {
                    longPollServer = TryUpdateLongPollServer(exception, longPollServer, configs);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                await Task.Delay(100, token);
            }
        }


        private LongPollServerResponse Auth(Configs configs)
        {
            LongPollServerResponse s;
            _api = new VkApi();
            _api.Authorize(new ApiAuthParams { AccessToken = configs.Token });
            s = _api.Groups.GetLongPollServer(configs.Id);
            return s;
        }

        private LongPollServerResponse TryUpdateLongPollServer(
            LongPollException exception,
            LongPollServerResponse s,
            Configs configs)
        {
            try
            {
                if (exception is LongPollOutdateException outdateException)
                    s.Ts = outdateException.Ts;
                else
                    s = _api.Groups.GetLongPollServer(configs.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                s = Auth(configs);
            }

            return s;
        }
    }
}