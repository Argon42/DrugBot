using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response.CommunityAnecdote;

public class CreateResponse(IAnecdoteProvider anecdoteProvider) : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var anecdote = message.Text.Remove(0, message.Text.IndexOf(' ') + 1).Trim();
        
        if (string.IsNullOrWhiteSpace(anecdote))
        {
            bot.SendMessage(message.CreateResponse("Ты где анек потерял?"));
            return;
        }
        
        anecdoteProvider.CreateNewAnecdote(message.User.Id, anecdote);
        
        bot.SendMessage(message.CreateResponse("Анекдот успешно создан"));
    }
}