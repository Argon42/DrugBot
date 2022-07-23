using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorWisdom : AbstractProcessor
    {
        private readonly List<string> keys = new List<string>
        {
            "/мудрость",
            "/цитата",
            "/бред",
            "!бред"
        };

        public override string Description =>
            $"Спроси у бота великую мудрость, для вызова используйте {string.Join(' ', keys)}";

        public override IReadOnlyList<string> Keys => keys;

        public override string Name => "Великие мудрости";


        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var path = "Local/wisdom.txt";
            var random = sentence.Length > 1 ? new Random(message.Text.GetHashCode()) : new Random();
            var lines = BotHandler.GetRandomLineFromFile(random, path, random.Next(2, 5));
            var line = string.Join(" ", lines);
            var words = line.Split(' ').OrderBy(s => random.Next()).ToList();

            var randomWordCount = random.Next(3, words.Count);
            var clampedCount = Math.Clamp(randomWordCount, 0, words.Count);
            var answer = string.Join(' ', words.Take(clampedCount).ToArray());
            BotHandler.SendMessage(vkApi, message.PeerId, answer);
        }
    }
}