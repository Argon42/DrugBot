namespace DrugBot.Bot.Vk;

public interface IVkMessage : IMessage<IVkMessage, IVkUser>
{
    long? ConversationMessageId { get; }
}