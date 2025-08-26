using DrugBot;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class WisdomResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        const string path = "Local/wisdom.txt";
        var random = message.Text.Split().Length > 1 ? new Random(message.Text.GetHashCode()) : new Random();
        var lines = BotHandler.GetRandomLineFromFile(random, path, random.Next(2, 5));
        var line = string.Join(" ", lines);
        var words = line.Split(' ').OrderBy(s => random.Next()).ToList();

        var randomWordCount = random.Next(3, words.Count);
        var clampedCount = Math.Clamp(randomWordCount, 0, words.Count);
        var answer = string.Join(' ', words.Take(clampedCount).ToArray());

        bot.SendMessage(message.CreateResponse(answer));
    }
}