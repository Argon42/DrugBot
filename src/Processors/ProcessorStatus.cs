using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorStatus : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            sentence.Length > 0 && string.Equals(sentence[0], "/статус", StringComparison.CurrentCultureIgnoreCase);

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            List<String> statuses;
            try
            {
                statuses = vkApi.Messages.GetConversationMembers(message.PeerId.Value, new String[] {"status"})
                    .Profiles
                    .Where(p => !string.IsNullOrEmpty(p.Status))
                    .Select(p => p.Status)
                    .ToList();
            }
            catch (VkNet.Exception.ConversationAccessDeniedException)
            {
                BotHandler.SendMessage(vkApi, message.PeerId,
                    "Для вывода случайного статуса участника, боту необходимы права администратора");
                return;
            }

            var rnd = new Random();
            var result = rnd.Next(0, statuses.Count());
            BotHandler.SendMessage(vkApi, message.PeerId, statuses[result]);
        }
    }
}