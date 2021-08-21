using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorWisdom : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "/мудрость",
            "/цитата"
        };

        public override string Name => "Великие мудрости";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Спроси у бота великую мудрость, для вызова используйте {string.Join(' ', keys)}";


        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var path = "Local/wisdom.txt";

            BotHandler.SendMessage(vkApi, message.PeerId, BotHandler.GetRandomLineFromFile(new Random(), path));
        }
    }
}