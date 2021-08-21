using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorPrediction : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "/8",
            "!8"
        };

        public override string Name => "Волшебный шар";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Хочешь получить ответ на вопрос, напиши: {string.Join(" | ", keys)} вопрос";


        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var path = "Local/8.txt";

            int seed = new Random().Next(int.MinValue, int.MaxValue);
            if (sentence.Length > 1)
                seed = BotHandler.GetDayUserSeed(message.PeerId) + message.Text.GetHashCode();

            BotHandler.SendMessage(vkApi, message.PeerId, BotHandler.GetRandomLineFromFile(new Random(seed), path));
        }
    }
}