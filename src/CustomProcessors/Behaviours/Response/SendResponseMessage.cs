using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

[Serializable]
internal class SendResponseMessage : Response
{
    public string Message { get; init; } = null!;
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        bot.SendMessage(message.CreateResponse(Message));
    }
}