using System;
using System.Collections.Generic;

namespace DrugBot.Processors;

[Processor]
public class ProcessorTry : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/try",
        "/попробовать",
        "/монетка"
    };

    public override string Description =>
        $"Если нужно бросить или попробовать что то сделать, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Пробователь";
    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        Random rnd = new();
        int result = rnd.Next();
        string s = result % 2 == 0 ? "Успех" : "Провал";
        
        bot.SendMessage(message.CreateResponse(s));
    }
}