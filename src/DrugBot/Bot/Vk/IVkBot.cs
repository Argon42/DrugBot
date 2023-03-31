using System.Threading;
using System.Threading.Tasks;

namespace DrugBot;

public interface IVkBot : IBot<IVkUser, IVkMessage>
{
    Task Start(CancellationToken token);
}