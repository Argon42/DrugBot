using System;
using System.Text;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorTotem : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            string.Equals(sentence[0], "/тотем", StringComparison.CurrentCultureIgnoreCase);

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));

            var stringBuilder = new StringBuilder("Сегодня вас ждет ");
            for (var i = 0; i < 5; i++)
                stringBuilder.Append($"&#{rnd.Next(127822, 129000)}; ");

            BotHandler.SendMessage(vkApi, message.PeerId, stringBuilder.ToString());
        }
    }
}