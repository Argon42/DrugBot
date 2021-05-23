using System;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorDa : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) => 
            string.Equals(message.Text, "да", StringComparison.CurrentCultureIgnoreCase);

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            BotHandler.SendMessage(vkApi, message.PeerId, $"Пизда");
        }
    }
}