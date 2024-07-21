using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.Processors;

[Processor]
public class ProcessorWisdom : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/мудрость",
        "/цитата",
        "/бред",
        "!бред",
    };

    public override string Description =>
        $"Спроси у бота великую мудрость, для вызова используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Великие мудрости";

    private readonly IWisdomDataProvider _wisdomDataProvider;

    public ProcessorWisdom(IWisdomDataProvider wisdomDataProvider)
    {
        _wisdomDataProvider = wisdomDataProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        var arrayCount = _wisdomDataProvider.GetArrayCount();
        var random = message.Text.Split().Length > 1 ? new Random(message.Text.GetHashCode()) : new Random();
        var count = random.Next(2, 5);
        var lines = new List<string>();

        for (var i = 0; i < count; i++)
        {
            lines.Add(_wisdomDataProvider.GetWisdom(random.Next(0, arrayCount - 1)));
        }
        
        var line = string.Join(" ", lines);
        var words = line.Split(' ').OrderBy(s => random.Next()).ToList();

        var randomWordCount = random.Next(3, words.Count);
        var clampedCount = Math.Clamp(randomWordCount, 0, words.Count);
        var answer = string.Join(' ', words.Take(clampedCount).ToArray());

        bot.SendMessage(message.CreateResponse(answer));
    }
}