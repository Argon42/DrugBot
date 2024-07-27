using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response.CommunityAnecdote;

public class ShowResponse(IAnecdoteProvider anecdoteProvider) : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var anecdote = anecdoteProvider.GetRandomAnecdote();

        if (anecdote == null)
        {
            bot.SendMessage(message.CreateResponse("Никто еще не создал анекдот. Будьте первым!"));
            return;
        }
        
        bot.SendMessage(message.CreateResponse(anecdote.Anecdote));
    }
}