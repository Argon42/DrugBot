using System.Text;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VkNet.Enums.Filters;

namespace BananvaBot
{
    class Program
    {
        private static DateTime? lastData;
        private static VkApi api = new VkApi();
        private static bool _botEnabled = true;

        static void Main(string[] args)
        {
            var configs = Configs.GetConfig();
            api.Authorize(new ApiAuthParams() {AccessToken = configs.Token});
            var s = api.Groups.GetLongPollServer(configs.Id);
            var botsLongPollHistoryParams = new BotsLongPollHistoryParams()
            {
                Server = s.Server,
                Ts = s.Ts,
                Key = s.Key,
                Wait = 25
            };
            var botHandler = new BotHandler(api);
            while (_botEnabled)
            {
                try
                {
                    var poll = api.Groups.GetBotsLongPollHistory(botsLongPollHistoryParams);

                    if (poll?.Updates == null || !poll.Updates.Any()) continue;
                    foreach (var a in poll.Updates)
                    {
                        Message message;
                        if (a.MessageNew != null)
                            message = a.MessageNew.Message;
                        else if (a.Message != null)
                            message = a.Message;
                        else
                            continue;

                        if (message.Date > lastData || lastData == null)
                        {
                            lastData = message.Date;
                            botHandler.MessageProcessing(message);
                        }
                    }
                }
                catch (LongPollException exception)
                {
                    if (exception is LongPollOutdateException outdateException)
                        s.Ts = outdateException.Ts;
                    else
                        s = api.Groups.GetLongPollServer(configs.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void Shutdown()
        {
            _botEnabled = false;
        }
    }
}