using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors.CommunityAnecdote;

[Processor]
public class ProcessorShow : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/анек-п"
    };
    
    public override string Description => $"Показать анекдот от сообщества. Команды: {string.Join(' ', _keys)}";
    public override string Name  => "Лучшие хохмы от сообщества";

    public override IReadOnlyList<string> Keys => _keys;

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        
    }
}