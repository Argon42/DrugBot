using System.Collections.Generic;
using System.Threading;
using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
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

    private IAnecdoteRepository _anecdoteRepository;

    public ProcessorCreate(IAnecdoteRepository anecdoteRepository)
    {
        _anecdoteRepository = anecdoteRepository;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        _anecdoteRepository.CreateNewAnecdote(1, message.Text);
        
        bot.SendMessage(message.CreateResponse("Анекдот успешно добавлен"));
    }
}