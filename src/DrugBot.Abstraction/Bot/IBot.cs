using VkNet.Model;

namespace DrugBot.Core.Bot;

public interface IBot<TUser, TMessage>
    where TUser : IUser
    where TMessage : IMessage<TMessage, TUser>
{
    long SendMessage(TMessage message);
    long EditMessage(long messageId, TMessage message);
    TUser? GetUser(string userName);
}