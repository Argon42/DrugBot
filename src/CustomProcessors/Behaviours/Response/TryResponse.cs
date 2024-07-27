using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class TryResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var rnd = new Random();
        var result = rnd.Next();
        var s = result % 2 == 0 ? "Успех" : "Провал";

        bot.SendMessage(message.CreateResponse(s));
    }
}