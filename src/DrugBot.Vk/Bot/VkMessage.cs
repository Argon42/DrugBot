using System;
using System.Collections.Generic;
using VkNet.Model;

namespace DrugBot.Vk.Bot;

internal class VkMessage : IVkMessage
{
    public long? ConversationMessageId { get; private init; }
    public IReadOnlyList<byte[]> Images { get; private init; }

    public string Text { get; private init; }
    public IVkMessage? TriggerMessage { get; private init; }

    public IVkUser User { get; private init; }

    private VkMessage()
    {
        Images = ArraySegment<byte[]>.Empty;
        Text = "";
        User = new VkUser(null, null, null, null, null);
    }

    public VkMessage(Message message)
    {
        Text = message.Text;
        User = new VkUser(message.FromId, message.PeerId, null, null, null);
        Images = ArraySegment<byte[]>.Empty;
        TriggerMessage = null;
        ConversationMessageId = message.ConversationMessageId;
    }

    public IVkMessage CreateResponse(string message = "", IReadOnlyList<byte[]>? images = null) =>
        new VkMessage
        {
            User = User,
            Text = message,
            Images = images ?? ArraySegment<byte[]>.Empty,
            ConversationMessageId = ConversationMessageId,
            TriggerMessage = this,
        };
}