using Anecdotes;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class AnecdoteResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var generator = message.Text.Split().Length > 1
            ? new AnecdoteGenerator(message.Text.GetHashCode())
            : new AnecdoteGenerator();

        bot.SendMessage(message.CreateResponse(generator.GenerateAnecdote().Anecdote));
    }
}