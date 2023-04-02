using DrugBot.Core.Bot;

namespace DrugBot.Core;

public interface IProcessor
{
    string Description { get; }
    string Name { get; }
    bool VisiblyDescription { get; }
    bool HasTrigger<TMessage>(TMessage message, string[] sentence) where TMessage : IMessage;

    bool TryProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
        where TUser : IUser
        where TMessage : IMessage<TMessage, TUser>;
}