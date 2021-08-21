using System;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace BananvaBot
{
    internal class Program
    {
        private static DateTime? _lastData;
        private static VkApi _api = new VkApi();
        private static bool _botEnabled = true;
        private static string _currentTs;

        private static void Main(string[] args)
        {
            Configs configs = Configs.GetConfig();
            LongPollServerResponse longPollServer = Auth(configs);
            var botHandler = new BotHandler(_api);
            _currentTs = longPollServer.Ts;
            while (_botEnabled)
            {
                try
                {
                    BotsLongPollHistoryResponse poll = _api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                    {
                        Server = longPollServer.Server,
                        Ts = _currentTs,
                        Key = longPollServer.Key,
                        Wait = 130
                    });

                    if (poll?.Updates == null || !poll.Updates.Any()) continue;

                    foreach (GroupUpdate a in poll.Updates)
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

                Task.Delay(100);
            }
        }

        private static LongPollServerResponse TryUpdateLongPollServer(
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

        private static LongPollServerResponse Auth(Configs configs)
        {
            LongPollServerResponse s;
            _api = new VkApi();
            _api.Authorize(new ApiAuthParams {AccessToken = configs.Token});
            s = _api.Groups.GetLongPollServer(configs.Id);
            return s;
        }

        public static void Shutdown()
        {
            _botEnabled = false;
        }
    }
}