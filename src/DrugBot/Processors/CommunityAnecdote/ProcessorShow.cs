using System.Collections.Generic;
using System.Threading;
using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
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
    
    private IAnecdoteProvider _anecdoteProvider;

    public ProcessorShow(IAnecdoteProvider anecdoteProvider)
    {
        _anecdoteProvider = anecdoteProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var anecdote = _anecdoteProvider.GetRandomAnecdote();

        if (anecdote == null)
        {
            bot.SendMessage(message.CreateResponse("Никто еще не создал анекдот. Будьте первым!"));
            return;
        }
        
        bot.SendMessage(message.CreateResponse(anecdote.Anecdote));
    }
}