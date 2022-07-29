using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors
{
    public class ProcessorQuote : AbstractProcessor
    {
        private readonly List<string> keys = new List<string>
        {
            "/фраза"
        };

        public override string Description =>
            $"Спроси у бота посредственную фразу, для вызова используйте {string.Join(' ', keys)}";

        public override IReadOnlyList<string> Keys => keys;

        public override string Name => "Рандомная цитата/фраза";


        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var path = "Local/wisdom.txt";
            var random = new Random();
            BotHandler.SendMessage(vkApi, message.PeerId, BotHandler.GetRandomLineFromFile(random, path));
        }
    }
}