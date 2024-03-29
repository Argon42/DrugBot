using System.Collections.Generic;
using System.Threading;
using Anecdotes;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorAnecdote : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/анек",
        "/анекдот",
        "/юмор",
        "/anek",
        "/хех",
    };

    public override string Description => $"Выдать случайный анекдот, для вызова используйте {string.Join(' ', _keys)}";
    public override IReadOnlyList<string> Keys => _keys;
    public override string Name => "Сборник баянов";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        AnecdoteGenerator generator = message.Text.Split().Length > 1
            ? new AnecdoteGenerator(message.Text.GetHashCode())
            : new AnecdoteGenerator();

        bot.SendMessage(message.CreateResponse(generator.GenerateAnecdote().Anecdote));
    }
}