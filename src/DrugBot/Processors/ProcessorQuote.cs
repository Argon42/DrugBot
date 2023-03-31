using System;
using System.Collections.Generic;
using DrugBot.Bot;
using DrugBot.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorQuote : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/фраза",
    };

    public override string Description =>
        $"Спроси у бота посредственную фразу, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Рандомная цитата/фраза";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        string path = "Local/wisdom.txt";
        Random random = new();
        string randomLineFromFile = BotHandler.GetRandomLineFromFile(random, path);
        bot.SendMessage(message.CreateResponse(randomLineFromFile));
    }
}