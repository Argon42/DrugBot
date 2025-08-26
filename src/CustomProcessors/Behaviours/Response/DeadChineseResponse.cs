using System.Text;
using DrugBot;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class DeadChineseResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var rnd = new Random(BotHandler.GetDayUserSeed(message.User.GetHashCode()));

        var predictionLength = (int)Math.Abs(15 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * rnd.NextDouble() - 1, 5)));
        var stringBuilder = new StringBuilder($"Мудрец видит что в будущем будет {GetPrediction(rnd, predictionLength)}");

        bot.SendMessage(message.CreateResponse(stringBuilder.ToString()));
    }
    
    private static string GetPrediction(Random rnd, int count)
    {
        try
        {
            const string path = "Local/chinese.txt";
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