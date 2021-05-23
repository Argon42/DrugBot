using System;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorTry : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            sentence.Length > 0 && sentence[0] == "/try";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random();
            var result = rnd.Next();
            BotHandler.SendMessage(vkApi, message.PeerId, result % 2 == 0 ? "Успех" : "Провал");
        }
    }
}