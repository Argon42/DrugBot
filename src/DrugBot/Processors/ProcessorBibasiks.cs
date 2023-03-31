using System;
using System.Collections.Generic;

namespace DrugBot.Processors;

[Processor]
public class ProcessorBibasiks : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/бибасики"
    };

    public override string Description =>
        $"Узнай размеры своих бибасиков, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Бибасикометр";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));

        float length = (float)rnd.NextDouble();
        double resultLenght = 80 + Math.Tan(0.5 * Math.PI * (2 * length - 1));

        string s = $"Сегодня ваши бибасики {resultLenght:F1} см в обхвате";
        bot.SendMessage(message.CreateResponse(s));
    }
}