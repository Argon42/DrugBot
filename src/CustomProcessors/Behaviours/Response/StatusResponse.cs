using DrugBot.Core.Bot;
using VkNet.Exception;

namespace CustomProcessors.Behaviours.Response;

public class StatusResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        List<string> statuses;
        
        try
        {
            statuses = bot.GetConversationMembers(message)
                .Where(p => !string.IsNullOrEmpty(p.Status))
                .Select(p => p.Status)
                .ToList();
        }
        catch (ConversationAccessDeniedException)
        {
            const string error = "Для вывода случайного статуса участника, боту необходимы права администратора";
            bot.SendMessage(message.CreateResponse(error));
            return;
        }

        if (statuses.Count == 0)
        {
            bot.SendMessage(message.CreateResponse("Нет пользователей со статусом в этом чате"));
            return;
        }
        
        var rnd = new Random();
        var result = rnd.Next(0, statuses.Count);
        var status = statuses[result];
        bot.SendMessage(message.CreateResponse(status));
    }
}