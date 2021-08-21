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
            try
            {
                if (vkApi.Users.Get(new[] {message.FromId.Value}, ProfileFields.Sex)[0].Sex ==
                    Sex.Female)
                {
                    BotHandler.SendMessage(vkApi, message.PeerId, "У вас нет бибы у вас бибасики");
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));

            double resultLenght = rnd.Next(-10, 40) + Math.Round(rnd.NextDouble(), 2);
            int resultDiameter = rnd.Next(20, 100);
            BotHandler.SendMessage(vkApi, message.PeerId,
                $"Сегодня ваша биба длиной {resultLenght} см и диаметром {resultDiameter} мм");
        }
    }
}