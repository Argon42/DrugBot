using DrugBot.Core.Bot;
using VkNet.Exception;

namespace CustomProcessors.Behaviours.Response;

public class WhoResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        List<string> names;
        
        try
        {
            names = bot.GetConversationMembers(message)
                .Select(user => $"{user.FirstName} {user.LastName}")
                .ToList();
        }
        catch (ConversationAccessDeniedException)
        {
            const string s = "Для вывода случайного статуса участника, боту необходимы права администратора";
            bot.SendMessage(message.CreateResponse(s));
            return;
        }

        var question = message.Text.Substring(message.Text.Split()[0].Length).TrimStart()
            .TrimEnd("?!".ToCharArray());
        var rnd = new Random(question.ToLower().GetHashCode());
        var result = rnd.Next(0, names.Count);
        var chanceOfNothing = rnd.NextDouble();
        var answer = $"{(chanceOfNothing > 0.9 ? "Никто не" : names[result])} {question}";
        bot.SendMessage(message.CreateResponse(answer));
    }
}