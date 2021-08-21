using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorDa : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "да"
        };

        public override string Name => "Да";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description => "Напиши \"Да\"";

        public override bool HasTrigger(Message message, string[] sentence) =>
            string.Equals(message.Text.TrimEnd(), "да", StringComparison.CurrentCultureIgnoreCase);

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            BotHandler.SendMessage(vkApi, message.PeerId, "Пизда");
        }
    }
}