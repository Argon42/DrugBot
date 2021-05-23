using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorBibasiks : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            string.Equals(sentence[0], "/бибасики", StringComparison.CurrentCultureIgnoreCase);

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            try
            {
                if (message.FromId != null &&
                    vkApi.Users.Get(new[] {message.FromId.Value}, ProfileFields.Sex)[0].Sex ==
                    VkNet.Enums.Sex.Male)
                {
                    BotHandler.SendMessage(vkApi, message.PeerId, $"У вас нет бибасиков у вас биба");
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            var result = rnd.Next(20, 150);
            BotHandler.SendMessage(vkApi, message.PeerId, $"Сегодня ваши бибасики {result} см в обхвате");
        }
    }
}