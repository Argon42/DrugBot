using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorShutdown : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) => 
            message.FromId == 175815456 && message.Text == "/BotStop";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            BotHandler.SendMessage(vkApi, message.PeerId, "Император сказал спать");
            Program.Shutdown();
        }
    }
}