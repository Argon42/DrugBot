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
        "/анек-c", //c английская
        "/anek-с", //с русская
        "/anek-c" //c английская
    };
    
    public override string Description => $"Создание скоего анекдота. Команды: {string.Join(' ', _keys)}";
    public override string Name  => "Создание своей хохмы";
    public override IReadOnlyList<string> Keys => _keys;

    private readonly IAnecdoteProvider _anecdoteProvider;

    public ProcessorCreate(IAnecdoteProvider anecdoteProvider)
    {
        _anecdoteProvider = anecdoteProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var anecdote = message.Text.Remove(0, message.Text.IndexOf(' ') + 1);
        
        if (string.IsNullOrWhiteSpace(anecdote))
        {
            bot.SendMessage(message.CreateResponse("Ты где анек потерял?"));
            return;
        }
        
        _anecdoteProvider.CreateNewAnecdote(message.User.Id, anecdote);
        
        bot.SendMessage(message.CreateResponse("Анекдот успешно создан"));
    }
}