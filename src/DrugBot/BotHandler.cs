using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrugBot.Bot;
using DrugBot.Processors;

namespace DrugBot;

public class BotHandler
{
    private readonly List<AbstractProcessor> _processors;

    public BotHandler(IEnumerable<AbstractProcessor> processors) => _processors = processors.ToList();

    public static int GetDayUserSeed(long? fromId)
    {
        if (fromId == null) return 0;

        int idHash = fromId.Value.GetHashCode();
        for (int i = 0; i < Math.Abs(DateTime.Today.GetHashCode()) % 10; i++)
            idHash = idHash.GetHashCode();

        int dateHash = DateTime.Today.GetHashCode();
        for (int i = 0; i < Math.Abs(DateTime.Today.GetHashCode()) % 10; i++)
            dateHash = dateHash.GetHashCode();

        return idHash + dateHash;
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

    public static bool IsBotTrigger(string s) => "@drugbot42," == s;

    public async Task MessageProcessing<TUser, TMessage>(TMessage message, IBot<TUser, TMessage> bot)
        where TUser : IUser
        where TMessage : IMessage<TMessage, TUser>
    {
        if (string.IsNullOrEmpty(message.Text)) return;

        await Task.Yield();
        try
        {
            string[] sentence = message.Text.ToLower().Split();
            AbstractProcessor? processor = _processors.FirstOrDefault(p => p.HasTrigger(message, sentence));
            processor?.TryProcessMessage(bot, message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}