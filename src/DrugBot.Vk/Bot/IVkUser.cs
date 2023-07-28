using DrugBot.Core.Bot;

namespace DrugBot.Vk.Bot;

public interface IVkUser : IUser
{
    long? PeerId { get; }
}