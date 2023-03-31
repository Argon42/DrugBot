namespace DrugBot;

public interface IVkMessage : IMessage<IVkMessage, IVkUser>
{
    long? ConversationMessageId { get; }
}