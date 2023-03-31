using System.Threading;
using System.Threading.Tasks;

namespace DrugBot.Bot.Vk;

public interface IVkBot : IBot<IVkUser, IVkMessage>
{
    Task Start(CancellationToken token);
}