using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DrugBot.Processors;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using System.Net;
using VkNet.Model.Attachments;

namespace DrugBot
{
    public class BotHandler
    {
        private readonly VkApi _api;
        private readonly List<AbstractProcessor> _processors;

        private static HttpClient Client { get; } = new();

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
                new AnecdoteProcessor(),
                new MemesProssessor(),
            };
            _processors.Add(new ProcessorHelp(_processors));
        }

        static BotHandler()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

        public static async void SendMessage(VkApi api, long? peerId, string message, byte[] image)
        {
            string response = await UploadPhoto(api, peerId, image);
            ReadOnlyCollection<Photo>? messagesPhoto = api.Photo.SaveMessagesPhoto(response);

            api.Messages.Send(new MessagesSendParams
            {
                PeerId = peerId,
                Message = message,
                RandomId = new Random().Next(),
                Attachments = new List<MediaAttachment?>()
                {
                    messagesPhoto.FirstOrDefault()
                }
            });
        }

        private static async Task<string> UploadPhoto(VkApi api, long? peerId, byte[] image)
        {
            MultipartFormDataContent content = new();
            UploadServerInfo? uploadServer = api.Photo.GetMessagesUploadServer(api.UserId.GetValueOrDefault());
            content.Add(new ByteArrayContent(image), "file", "photo.jpg");
            HttpResponseMessage responseMessage = await Client.PostAsync(uploadServer.UploadUrl, content);
            byte[] responseRaw = await responseMessage.Content.ReadAsByteArrayAsync();
            return Encoding.GetEncoding(1251).GetString(responseRaw);
        }
    }
}