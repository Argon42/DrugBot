using System;
using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorQuote : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/фраза",
    };

    public override string Description =>
        $"Спроси у бота посредственную фразу, для вызова используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Рандомная цитата/фраза";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        string path = "Local/wisdom.txt";
        Random random = new();
        string randomLineFromFile = BotHandler.GetRandomLineFromFile(random, path);
        bot.SendMessage(message.CreateResponse(randomLineFromFile));
    }
}