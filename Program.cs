using System.Text;
using System.Collections.Generic;
using System;
using System.Globalization;
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

        static void Main(string[] args)
        {
            var configs = Configs.GetConfig();
            api.Authorize(new ApiAuthParams() { AccessToken = configs.Token });
            var s = api.Groups.GetLongPollServer(configs.Id);
            while (true)
            {
                try
                {
                    var poll = api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                    {
                        Server = s.Server,
                        Ts = s.Ts,
                        Key = s.Key,
                        Wait = 25
                    });

                    if (poll?.Updates == null || poll.Updates.Count() == 0) continue;
                    foreach (var a in poll.Updates)
                    {
                        Message message;
                        if (a.MessageNew != null)
                        {
                            message = a.MessageNew.Message;
                        }
                        else if (a.Message != null)
                        {
                            message = a.Message;
                        }
                        else continue;

                        if (message.Date > lastData || lastData == null)
                        {
                            lastData = message.Date;
                            MessageProcessing(message);
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

        private static void MessageProcessing(Message message)
        {
            if (String.IsNullOrEmpty(message.Text) || message.Text.Length < 3)
                return;

            if (!message.Text.Contains('/'))
                return;

            Console.WriteLine($"{message.FromId}: {message.Text}");

            #region Dice

            var sentence = message.Text.ToLower().Split();
            if (sentence[0] == "/dice" && sentence.Length >= 2)
            {
                var dices = sentence[1].Split('d');
                if (!int.TryParse(dices[0], out int diceCount))
                    return;
                if (!int.TryParse(dices[1], out int diceValue))
                    return;
                if (diceValue < 0 || diceCount < 0)
                    return;

                var modificator = 0;
                if (sentence.Length >= 3)
                    int.TryParse(sentence[2], out modificator);

                var rnd = new Random();
                var result = rnd.Next(diceCount, diceCount * diceValue + 1);
                result += modificator;
                SendMessage(api, message.PeerId, $"В сумме выпало {result}");
            }

            #endregion

            #region Биба

            if (sentence[0] == "/биба" || sentence[0] == "/biba" || (sentence.Length >= 2 && sentence[1] == "/биба"))
            {
                try
                {
                    if (api.Users.Get(new long[] { message.FromId.Value }, ProfileFields.Sex)[0].Sex == VkNet.Enums.Sex.Female)
                    {
                        SendMessage(api, message.PeerId, $"У вас нет бибы у вас бибасики");
                        return;
                    }
                }
                catch (Exception) { }
                var rnd = new Random(GetDayUserSeed(message.FromId));
                var resultLenght = rnd.Next(-10, 40) + Math.Round(rnd.NextDouble(), 2);
                var resultDiametr = rnd.Next(20, 100);
                var resultDiametrMeasures = rnd.Next();
                SendMessage(api, message.PeerId, $"Сегодня ваша биба длиной {resultLenght} см и диаметром {resultDiametr} мм");
            }

            #endregion

            #region Тотем

            if (sentence[0] == "/тотем" || (sentence.Length >= 2 && sentence[1] == "/тотем"))
            {
                var rnd = new Random(GetDayUserSeed(message.FromId));
                StringBuilder str = new StringBuilder("Сегодня вас ждет ");
                for (int i = 0; i < 5; i++)
                    str.Append($"&#{rnd.Next(127822, 129000)}; ");
                SendMessage(api, message.PeerId, str.ToString());
            }

            #endregion

            #region Женская биба

            if (sentence[0] == "/бибасики")
            {
                try
                {
                    if (api.Users.Get(new long[] { message.FromId.Value }, ProfileFields.Sex)[0].Sex == VkNet.Enums.Sex.Male)
                    {
                        SendMessage(api, message.PeerId, $"У вас нет бибасиков у вас биба");
                        return;
                    }
                }
                catch (Exception) { }
                var rnd = new Random(GetDayUserSeed(message.FromId));
                var result = rnd.Next(20, 150);
                SendMessage(api, message.PeerId, $"Сегодня ваши бибасики {result} см в обхвате");
            }

            #endregion

            #region Рандом статус

            if (sentence[0] == "/статус")
            {
                List<String> statuses;
                try
                {
                    statuses = api.Messages.GetConversationMembers(message.PeerId.Value, new String[] { "status" })
                    .Profiles
                    .Where(p => !String.IsNullOrEmpty(p.Status))
                    .Select(p => p.Status)
                    .ToList();
                }
                catch (VkNet.Exception.ConversationAccessDeniedException)
                {
                    SendMessage(api, message.PeerId, "Для вывода случайного статуса участника, боту необходимы права администратора");
                    return;
                }
                var rnd = new Random();
                var result = rnd.Next(0, statuses.Count());
                SendMessage(api, message.PeerId, statuses[result]);
            }

            #endregion

            #region Попытка

            if (sentence[0] == "/try")
            {
                var rnd = new Random();
                var result = rnd.Next();
                SendMessage(api, message.PeerId, result % 2 == 0 ? "Успех" : "Провал");
            }

            #endregion

            #region Zargo

            if (sentence[0] == "/zargo" || (sentence.Length >= 2 && sentence[1] == "/zargo"))
            {


            }

            #endregion

        }

        private static int GetDayUserSeed(long? fromId)
        {
            var idHash = fromId.Value.GetHashCode();
            var dateHash = (DateTime.Now.Day.GetHashCode()
                + DateTime.Now.Month.GetHashCode()
                + DateTime.Now.Year.GetHashCode());
            return idHash + dateHash;
        }

        private static void SendMessage(VkApi api, long? peerId, string message)
        {
            api.Messages.Send(new MessagesSendParams()
            {
                PeerId = peerId,
                Message = message,
                RandomId = new Random().Next()
            });
        }
    }
}