using DrugBot.Core.Bot;

namespace DrugBot.Tests;

public class MessageStab : IMessage<MessageStab, UserStab>
{
    public string Text { get; set; } = null!;
    public IReadOnlyList<byte[]> Images { get; set; } = null!;
    public MessageStab TriggerMessage { get; set; } = null!;
    public UserStab User { get; set; } = null!;

    public MessageStab CreateResponse(string message = "", IReadOnlyList<byte[]>? images = null)
    {
        return new MessageStab
        {
            Text = message,
            Images = images ?? ArraySegment<byte[]>.Empty
        };
    }
}