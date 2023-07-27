using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public abstract class Response : BaseBehaviour
{
    public abstract void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token) 
        where TUser : IUser 
        where TMessage : IMessage<TMessage, TUser>;
}