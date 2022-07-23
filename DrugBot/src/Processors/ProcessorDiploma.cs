using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors
{
    public class ProcessorDiploma : AbstractProcessor
    {
        private readonly List<string> keys = new List<string>
        {
            "/диплом"
        };

        public override string Description =>
            $"Экстрасенсорный анализ диплома и бонус, для вызова используйте {string.Join(' ', keys)}";

        public override IReadOnlyList<string> Keys => keys;

        public override string Name => "дИПЛОМОЗИТОР";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            var pages = rnd.Next(-3, 130);
            var originality = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
            var chanceOfSurrender = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
            var prediction = GetPrediction(rnd);

            var result = $"[id{message.FromId}|Ваш] диплом состоит из {pages} страниц(ы) , \n" +
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