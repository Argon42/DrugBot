using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors
{
    public class ProcessorWisdom : AbstractProcessor
    {
        private readonly List<string> _keys = new List<string>
        {
            "/мудрость",
            "/цитата",
            "/бред",
            "!бред"
        };

        public override string Description =>
            $"Спроси у бота великую мудрость, для вызова используйте {string.Join(' ', _keys)}";

        public override IReadOnlyList<string> Keys => _keys;

        public override string Name => "Великие мудрости";


        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            string path = "Local/wisdom.txt";
            Random random = sentence.Length > 1 ? new Random(message.Text.GetHashCode()) : new Random();
            List<string> lines = BotHandler.GetRandomLineFromFile(random, path, random.Next(2, 5));
            string line = string.Join(" ", lines);
            List<string> words = line.Split(' ').OrderBy(s => random.Next()).ToList();

            int randomWordCount = random.Next(3, words.Count);
            int clampedCount = Math.Clamp(randomWordCount, 0, words.Count);
            string answer = string.Join(' ', words.Take(clampedCount).ToArray());
            BotHandler.SendMessage(vkApi, message.PeerId, answer);
        }
    }
}