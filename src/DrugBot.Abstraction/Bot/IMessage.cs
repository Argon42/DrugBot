namespace DrugBot.Core.Bot;

public interface IMessage<TMessage, TUser> : IMessage where TUser : IUser
{
    IReadOnlyList<byte[]> Images { get; }
    TMessage TriggerMessage { get; }
    TUser User { get; }
    TMessage CreateResponse(string message = "", IReadOnlyList<byte[]>? images = null);
}

public interface IMessage
{
    string Text { get; }
}