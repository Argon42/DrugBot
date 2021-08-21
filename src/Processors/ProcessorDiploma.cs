using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorDiploma : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "/диплом"
        };

        public override string Name => "дИПЛОМОЗИТОР";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Экстрасенсорный анализ диплома и бонус, для вызова используйте {string.Join(' ', keys)}";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            int pages = rnd.Next(-3, 130);
            double originality = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
            double chanceOfSurrender = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
            string prediction = GetPrediction(rnd);

            string result = $"[id{message.FromId}|Ваш] диплом состоит из {pages} страниц(ы) , \n" +
                            $"текущая оригинальность {originality}%, \n" +
                            $"шанс сдать = {chanceOfSurrender}%\n" +
                            $"Предсказание к диплому: {prediction}";
            BotHandler.SendMessage(vkApi, message.PeerId, result);
        }

        private static string GetPrediction(Random rnd)
        {
            try
            {
                var path = "Local/predictions.txt";
                return BotHandler.GetRandomLineFromFile(rnd, path);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}