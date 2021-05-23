using System;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorDiploma : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            string.Equals(message.Text, "/диплом", StringComparison.CurrentCultureIgnoreCase);
        
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
                string path = "Local/predictions.txt";
                return BotHandler.GetRandomLineFromFile(rnd, path);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}