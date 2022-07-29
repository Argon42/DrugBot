using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors
{
    public class ProcessorBiba : AbstractProcessor
    {
        private readonly List<string> keys = new List<string>
        {
            "/биба",
            "/biba",
            "/bebra",
            "/бебра"
        };

        public override string Description =>
            $"Узнай размеры своей бибы, для вызова используйте {string.Join(' ', keys)}";

        public override IReadOnlyList<string> Keys => keys;

        public override string Name => "Бибометр";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            var maleBiba = rnd.NextDouble() < 0.46f;

            var length = (float)rnd.NextDouble();
            var resultLenght = maleBiba
                ? 15 + Math.Tan(0.5 * Math.PI * (2 * length - 1))
                : -4 + Math.Tan(0.5 * Math.PI * (2 * length - 1));

            var diameter = (float)rnd.NextDouble();
            var resultDiameter = 30 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * diameter - 1, 1));

            BotHandler.SendMessage(vkApi, message.PeerId,
                $"Сегодня ваша биба ({(maleBiba ? "male" : "female")}) длиной {resultLenght:F2} см и диаметром {resultDiameter:F2} мм");
        }
    }
}