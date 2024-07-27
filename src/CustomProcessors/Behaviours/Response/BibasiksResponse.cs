using DrugBot;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class BibasiksResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var rnd = new Random(BotHandler.GetDayUserSeed(message.User.GetHashCode()));

        var length = (float)rnd.NextDouble();
        var resultLenght = 80 + Math.Tan(0.5 * Math.PI * (2 * length - 1));

        var s = $"Сегодня ваши бибасики {resultLenght:F1} см в обхвате";
        bot.SendMessage(message.CreateResponse(s));
    }
}