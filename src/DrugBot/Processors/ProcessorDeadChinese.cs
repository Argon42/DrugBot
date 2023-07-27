using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorDeadChinese : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/мудрец",
    };

    public override string Description =>
        $"Пророчество от мудрого, но мертвого китайца, для вызова деда используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Мертвый китаец";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));

        int predictionLength = (int)Math.Abs(15 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * rnd.NextDouble() - 1, 5)));
        StringBuilder stringBuilder = new($"Мудрец видит что в будущем будет {GetPrediction(rnd, predictionLength)}");

        bot.SendMessage(message.CreateResponse(stringBuilder.ToString()));
    }

    private static string GetPrediction(Random rnd, int count)
    {
        try
        {
            string path = "Local/chinese.txt";
            StringBuilder builder = new();

            List<string> predictions = File.ReadLines(path).ToList();
            for (int i = 0; i < count; i++)
                builder.AppendJoin(' ', predictions[rnd.Next(0, predictions.Count)]);

            return builder.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}