namespace DrugBot;

public interface IVkUser : IUser
{
    long? PeerId { get; }
}