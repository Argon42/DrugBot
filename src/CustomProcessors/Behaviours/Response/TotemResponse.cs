using System.Text;
using DrugBot;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class TotemResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));
        var stringBuilder = new StringBuilder($"Сегодня вас ждет {GetPrediction(rnd, rnd.Next(3, 6))}");

        bot.SendMessage(message.CreateResponse(stringBuilder.ToString()));
    }
    
    private static string GetPrediction(Random rnd, int count)
    {
        try
        {
            const string path = "Local/emodzy.txt";
            return string.Join(' ', BotHandler.GetRandomLineFromFile(rnd, path, count));
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}