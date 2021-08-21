using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorTry : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "/try",
            "/попробовать",
            "/монетка"
        };

        public override string Name => "Пробователь";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Если нужно бросить или попробовать что то сделать, для вызова используйте {string.Join(' ', keys)}";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random();
            int result = rnd.Next();
            BotHandler.SendMessage(vkApi, message.PeerId, result % 2 == 0 ? "Успех" : "Провал");
        }
    }
}