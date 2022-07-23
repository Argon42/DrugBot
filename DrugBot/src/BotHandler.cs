using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DrugBot.Processors;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace DrugBot
{
    public class BotHandler
    {
        private readonly VkApi _api;
        private readonly List<AbstractProcessor> _processors;

        public BotHandler(VkApi api)
        {
            _api = api;
            _processors = new List<AbstractProcessor>
            {
                new ProcessorTry(),
                new ProcessorDa(),
                new ProcessorPrediction(),
                new ProcessorDiploma(),
                new ProcessorStatus(),
                new ProcessorBibasiks(),
                new ProcessorTotem(),
                new ProcessorBiba(),
                new ProcessorDice(),
                new ProcessorWho(),
                new ProcessorWisdom(),
                new ProcessorQuote(),
                new ProcessorDeadChinese(),
            };
            _processors.Add(new ProcessorHelp(_processors));
        }

        public static int GetDayUserSeed(long? fromId)
        {
            if (fromId == null) return 0;

            var idHash = fromId.Value.GetHashCode();
            for (var i = 0; i < Math.Abs(DateTime.Today.GetHashCode()) % 10; i++)
                idHash = idHash.GetHashCode();

            var dateHash = DateTime.Today.GetHashCode();
            for (var i = 0; i < Math.Abs(DateTime.Today.GetHashCode()) % 10; i++)
                dateHash = dateHash.GetHashCode();

            return idHash + dateHash;
        }

        public static string GetRandomLineFromFile(Random rnd, string path)
        {
            var predictions = File.ReadLines(path).ToList();
            var prediction = predictions[rnd.Next(0, predictions.Count)];
            return prediction;
        }

        public static List<string> GetRandomLineFromFile(Random rnd, string path, int count)
        {
            var predictions = File.ReadLines(path).ToList();
            return predictions.OrderBy(s => rnd.NextDouble()).Take(count).ToList();
        }

        public static bool IsBotTrigger(string s)
        {
            return "@drugbot42," == s;
        }

        public void MessageProcessing(Message message)
        {
            if (string.IsNullOrEmpty(message.Text)) return;

            var sentence = message.Text.ToLower().Split();
            var processor = _processors.FirstOrDefault(p => p.HasTrigger(message, sentence));
            processor?.TryProcessMessage(_api, message, sentence);
        }

        public static void SendMessage(VkApi api, long? peerId, string message)
        {
            api.Messages.Send(new MessagesSendParams
            {
                PeerId = peerId,
                Message = message,
                RandomId = new Random().Next()
            });
        }
    }
}