using DrugBot.Core;
using DrugBot.Core.Bot;
using Microsoft.Extensions.DependencyInjection;

namespace CustomProcessors.Behaviours.Response;

public class HelpResponse(IServiceProvider serviceProvider) : Response
{
    private IReadOnlyList<IProcessor>? _processors;
    
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        _processors ??= serviceProvider.GetServices<IProcessor>().ToList();

        var answer = string.Join('\n',
            _processors.Where(processor => processor.VisiblyDescription)
                .Select(processor => $"{processor.Name}\n{processor.Description}\n")
        );
        
        bot.SendMessage(message.CreateResponse(answer));
    }
}