using System;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorPrediction : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            sentence[0] == "/8";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var path = "Local/8.txt";

            var seed = new Random().Next(int.MinValue, int.MaxValue);
            if (sentence.Length > 1)
                seed = BotHandler.GetDayUserSeed(message.PeerId) + message.Text.GetHashCode();

            BotHandler.SendMessage(vkApi, message.PeerId, BotHandler.GetRandomLineFromFile(new Random(seed), path));
        }
    }
}