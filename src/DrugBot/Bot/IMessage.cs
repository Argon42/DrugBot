using System.Collections.Generic;

namespace DrugBot;

public interface IMessage<TMessage, TUser> : IMessage where TUser : IUser
{
    TUser User { get; }
    IReadOnlyList<byte[]> Images { get; }
    TMessage TriggerMessage { get; }
    TMessage CreateResponse(string message = "", IReadOnlyList<byte[]>? images = null);
}

public interface IMessage
{
    string Text { get; }
}