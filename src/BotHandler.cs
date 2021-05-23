using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BananvaBot
{
    public class BotHandler
    {
        private readonly VkApi _api;
        private readonly List<AbstractProcessor> _processors;

        public BotHandler(VkApi api)
        {
            _api = api;
            _processors = new List<AbstractProcessor>()
            {
                new ProcessorTry(),
                new ProcessorDa(),
                new ProcessorShutdown(),
                new ProcessorPrediction(),
                new ProcessorDiploma(),
                new ProcessorStatus(),
                new ProcessorBibasiks(),
                new ProcessorTotem(),
                new ProcessorBiba(),
                new ProcessorDice()
            };
        }

        public void MessageProcessing(Message message)
        {
            if (string.IsNullOrEmpty(message.Text)) return;

            var sentence = message.Text.ToLower().Split();
            var processor = _processors.FirstOrDefault(p => p.HasTrigger(message, sentence));
            processor?.TryProcessMessage(_api, message, sentence);
        }

        public static string GetRandomLineFromFile(Random rnd, string path)
        {
            var predictions = File.ReadLines(path).ToList();
            var prediction = predictions[rnd.Next(0, predictions.Count)];
            return prediction;
        }

        public static int GetDayUserSeed(long? fromId)
        {
            if (fromId == null) return 0;

            var idHash = fromId.Value.GetHashCode();
            var dateHash = (DateTime.Now.Day.GetHashCode()
                            + DateTime.Now.Month.GetHashCode()
                            + DateTime.Now.Year.GetHashCode());
            return idHash + dateHash;
        }

        public static void SendMessage(VkApi api, long? peerId, string message)
        {
            api.Messages.Send(new MessagesSendParams()
            {
                PeerId = peerId,
                Message = message,
                RandomId = new Random().Next()
            });
        }
    }
}