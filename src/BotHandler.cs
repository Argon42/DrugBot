using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VkNet;
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
            _processors = new List<AbstractProcessor>
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
                new ProcessorDice(),
                new ProcessorWho(),
                new ProcessorWisdom()
            };
            _processors.Add(new ProcessorHelp(_processors));
        }

        public void MessageProcessing(Message message)
        {
            if (string.IsNullOrEmpty(message.Text)) return;

            string[] sentence = message.Text.ToLower().Split();
            AbstractProcessor processor = _processors.FirstOrDefault(p => p.HasTrigger(message, sentence));
            processor?.TryProcessMessage(_api, message, sentence);
        }

        public static string GetRandomLineFromFile(Random rnd, string path)
        {
            List<string> predictions = File.ReadLines(path).ToList();
            string prediction = predictions[rnd.Next(0, predictions.Count)];
            return prediction;
        }

        public static List<string> GetRandomLineFromFile(Random rnd, string path, int count)
        {
            List<string> predictions = File.ReadLines(path).ToList();
            return predictions.OrderBy(s => rnd.NextDouble()).Take(count).ToList();
        }

        public static int GetDayUserSeed(long? fromId)
        {
            if (fromId == null) return 0;

            int idHash = fromId.Value.GetHashCode();
            int dateHash = DateTime.Now.Day.GetHashCode()
                           + DateTime.Now.Month.GetHashCode()
                           + DateTime.Now.Year.GetHashCode();
            return idHash + dateHash;
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

        public static bool IsBotTrigger(string s) => "@drugbot42," == s;
    }
}