namespace DrugBot.Core.Bot;

public interface IBot<TUser, TMessage>
    where TUser : IUser
    where TMessage : IMessage<TMessage, TUser>
{
    void SendMessage(TMessage message);
}