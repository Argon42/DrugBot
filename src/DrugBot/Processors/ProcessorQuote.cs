using System;
using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using DrugBot.DataBase.DataProviders.Interfaces;

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

    private readonly IWisdomDataProvider _wisdomDataProvider;

    public ProcessorQuote(IWisdomDataProvider wisdomDataProvider)
    {
        _wisdomDataProvider = wisdomDataProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        Random random = new();
        var arrayCount = _wisdomDataProvider.GetArrayCount();
        var wisdom = _wisdomDataProvider.GetWisdom(random.Next(0, arrayCount - 1));
        
        bot.SendMessage(message.CreateResponse(wisdom));
    }
}