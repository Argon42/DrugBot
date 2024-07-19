using System;
using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorBiba : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/биба",
        "/biba",
        "/bebra",
        "/бебра",
    };

    public override string Description =>
        $"Узнай размеры своей бибы, для вызова используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Бибометр";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));
        bool maleBiba = rnd.NextDouble() < 0.46f;

        float length = (float)rnd.NextDouble();
        double resultLength = maleBiba
            ? 15 + Math.Tan(0.5 * Math.PI * (2 * length - 1))
            : -4 + Math.Tan(0.5 * Math.PI * (2 * length - 1));

        float diameter = (float)rnd.NextDouble();
        double resultDiameter = 30 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * diameter - 1, 1));

        string s = string.Format("Сегодня ваша биба ({0}) длиной {1:F2} см и диаметром {2:F2} мм",
                maleBiba 
                    ? "male" 
                    : "female", 
                resultLength, 
                resultDiameter);
        bot.SendMessage(message.CreateResponse(s));
    }
}