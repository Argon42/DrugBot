using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorDeadChinese : AbstractProcessor
    {
        private readonly List<string> keys = new List<string>
        {
            "/мудрец"
        };

        public override string Description =>
            $"Пророчество от мудрого, но мертвого китайца, для вызова деда используйте {string.Join(' ', keys)}";

        public override IReadOnlyList<string> Keys => keys;

        public override string Name => "Мертвый китаец";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));


            var predictionLength = (int)Math.Abs(15 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * rnd.NextDouble() - 1, 5)));
            var stringBuilder =
                new StringBuilder($"Мудрец видит что в будущем будет {GetPrediction(rnd, predictionLength)}");

            BotHandler.SendMessage(vkApi, message.PeerId, stringBuilder.ToString());
        }

        private static string GetPrediction(Random rnd, int count)
        {
            try
            {
                var path = "Local/chinese.txt";
                var builder = new StringBuilder();

                var predictions = File.ReadLines(path).ToList();
                for (var i = 0; i < count; i++)
                    builder.AppendJoin(' ', predictions[rnd.Next(0, predictions.Count)]);

                return builder.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}