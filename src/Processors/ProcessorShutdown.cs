using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorShutdown : AbstractProcessor
    {
        public override bool VisiblyDescription => false;
        public override string Name => "Выключение бота";
        public override IReadOnlyList<string> Keys => null;
        public override string Description => "Отключение бота, доступно только для Императора";

        public override bool HasTrigger(Message message, string[] sentence) =>
            message.FromId == 175815456 && message.Text == "/BotStop";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            BotHandler.SendMessage(vkApi, message.PeerId, "Император сказал спать");
            Program.Shutdown();
        }
    }
}