using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

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

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        string path = "Local/wisdom.txt";
        Random random = message.Text.Split().Length > 1 ? new Random(message.Text.GetHashCode()) : new Random();
        List<string> lines = BotHandler.GetRandomLineFromFile(random, path, random.Next(2, 5));
        string line = string.Join(" ", lines);
        List<string> words = line.Split(' ').OrderBy(s => random.Next()).ToList();

        int randomWordCount = random.Next(3, words.Count);
        int clampedCount = Math.Clamp(randomWordCount, 0, words.Count);
        string answer = string.Join(' ', words.Take(clampedCount).ToArray());

        bot.SendMessage(message.CreateResponse(answer));
    }
}