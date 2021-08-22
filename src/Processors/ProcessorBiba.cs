using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorBiba : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "/биба",
            "/biba"
        };

        public override string Name => "Бибометр";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Узнай размеры своей бибы, для вызова используйте {string.Join(' ', keys)}";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            bool maleBiba = rnd.NextDouble() < 0.46f;

            var length = (float) rnd.NextDouble();
            double resultLenght = maleBiba
                ? 15 + Math.Tan(0.5 * Math.PI * (2 * length - 1))
                : -4 + Math.Tan(0.5 * Math.PI * (2 * length - 1));

            var diameter = (float) rnd.NextDouble();
            double resultDiameter = 30 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * diameter - 1, 1));
            
            BotHandler.SendMessage(vkApi, message.PeerId,
                $"Сегодня ваша биба длиной {resultLenght:F2} см и диаметром {resultDiameter:F2} мм");
        }
    }
}