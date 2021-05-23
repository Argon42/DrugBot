using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorBiba : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            string.Equals(sentence[0], "/биба", StringComparison.CurrentCultureIgnoreCase) ||
            string.Equals(sentence[0], "/biba", StringComparison.CurrentCultureIgnoreCase);

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            try
            {
                if (vkApi.Users.Get(new long[] {message.FromId.Value}, ProfileFields.Sex)[0].Sex ==
                    VkNet.Enums.Sex.Female)
                {
                    BotHandler.SendMessage(vkApi, message.PeerId, $"У вас нет бибы у вас бибасики");
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            var resultLenght = rnd.Next(-10, 40) + Math.Round(rnd.NextDouble(), 2);
            var resultDiameter = rnd.Next(20, 100);
            BotHandler.SendMessage(vkApi, message.PeerId,
                $"Сегодня ваша биба длиной {resultLenght} см и диаметром {resultDiameter} мм");
        }
    }
}