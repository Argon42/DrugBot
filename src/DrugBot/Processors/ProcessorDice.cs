using System;
using System.Collections.Generic;
using System.Linq;
using DrugBot.Bot;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorDice : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/dice",
        "/d",
        "/кости",
        "/кость",
        "/к",
    };

    public override string Description =>
        "Узнай размеры своей бибы, для вызова используйте:\n" +
        $"{string.Join('\n', keys.Select(k => $"{k} XdY Z"))}\n" +
        "где X число костей\n" +
        "Y колличество граней\n" +
        "Z модификатор который будет добавлен к числу, например: -2 | 6 (по стандарту 0)";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Кости";

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence)
    {
        return sentence.Length >= 2 &&
               keys.Any(s => s.Equals(sentence[0], StringComparison.CurrentCultureIgnoreCase)) &&
               sentence[1].Split('d').Length == 2;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        string[] sentence = message.Text.Split();
        string[] dices = sentence[1].Split('d');
        if (!int.TryParse(dices[0], out int diceCount)) return;
        if (!int.TryParse(dices[1], out int diceValue)) return;
        if (diceValue < 0 || diceCount < 0) return;

        int modificator = 0;
        if (sentence.Length >= 3)
            int.TryParse(sentence[2], out modificator);

        Random rnd = new();

        int result = 0;
        for (int i = 0; i < diceCount; i++)
            result += rnd.Next(0, diceValue) + 1;

        bot.SendMessage(message.CreateResponse($"Выпало {result} + {modificator} = {result + modificator}"));
    }
}