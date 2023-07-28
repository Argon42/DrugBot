using DrugBot.Core.Bot;

namespace DrugBot.Vk.Bot;

public interface IVkMessage : IMessage<IVkMessage, IVkUser>
{
    long? ConversationMessageId { get; }
}