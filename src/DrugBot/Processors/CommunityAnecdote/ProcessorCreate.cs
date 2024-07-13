using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors.CommunityAnecdote;

[Processor]
public class ProcessorCreate : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/анек-с", //с русская
        "/анек-c" //c английская
    };
    
    public override string Description => $"Создание скоего анекдота. Команды: {string.Join(' ', _keys)}";
    public override string Name  => "Создание своей хохмы";

    public override IReadOnlyList<string> Keys => _keys;

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        
    }
}