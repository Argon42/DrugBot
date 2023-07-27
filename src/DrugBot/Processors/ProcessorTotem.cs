using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DrugBot.Bot;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorTotem : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/тотем",
    };

    public override string Description =>
        $"Посмотреть за ширму вселенной и узнать что тебя сегодня ждет, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Тотем дня";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));
        StringBuilder stringBuilder = new($"Сегодня вас ждет {GetPrediction(rnd, rnd.Next(3, 6))}");

        bot.SendMessage(message.CreateResponse(stringBuilder.ToString()));
    }

    private static string GetPrediction(Random rnd, int count)
    {
        try
        {
            string path = "Local/emodzy.txt";
            return string.Join(' ', BotHandler.GetRandomLineFromFile(rnd, path, count));
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}