using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response.CommunityAnecdote;

public class ShowUserResponse(IAnecdoteProvider anecdoteProvider) : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var rawQuery = message.Text.Remove(0, message.Text.IndexOf(' ') + 1).Trim().Split(' ');

        if (rawQuery.Length != 1)
        {
            bot.SendMessage(
                message.CreateResponse("Некорректный запрос. \n Пример правильного запроса: \"/анек-п-п bertad\""));
            return;
        }

        var query = rawQuery[0];
        var user = bot.GetUser(query);

        if (user == null)
        {
            bot.SendMessage(message.CreateResponse($"Юморист с именем {query} не найден"));
            return;
        }
        
        var anecdote = anecdoteProvider.GetRandomAnecdoteFromUser(user.Id);

        if (anecdote == null)
        {
            bot.SendMessage(message.CreateResponse("Этот юморист еще не создал ни одного анекдота."));
            return;
        }
        
        bot.SendMessage(message.CreateResponse(anecdote.Anecdote));
    }
}