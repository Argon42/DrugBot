using System;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorDice : AbstractProcessor
    {
        public override bool HasTrigger(Message message, string[] sentence) =>
            sentence.Length >= 2 &&
            (sentence[0] == "/dice" || sentence[0] == "/d") &&
            sentence[1].Split('d').Length == 2;

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var dices = sentence[1].Split('d');
            if (!int.TryParse(dices[0], out var diceCount)) return;
            if (!int.TryParse(dices[1], out var diceValue)) return;
            if (diceValue < 0 || diceCount < 0) return;

            var modificator = 0;
            if (sentence.Length >= 3)
                int.TryParse(sentence[2], out modificator);

            var rnd = new Random();

            var result = 0;
            for (var i = 0; i < diceCount; i++)
                result += rnd.Next(0, diceValue) + 1;

            result += modificator;
            BotHandler.SendMessage(vkApi, message.PeerId, $"В сумме выпало {result}");
        }
    }
}