using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DrugBot.Processors;

[Processor]
public class ProcessorHelp : AbstractProcessor
{
    private IReadOnlyList<AbstractProcessor>? _processors;

    private readonly List<string> keys = new()
    {
        "/help",
        "!help",
        "/помощь",
        "!помощь"
    };

    private readonly IServiceProvider _serviceProvider;

    public override string Description => "тутор со всеми командами";
    public override IReadOnlyList<string> Keys => keys;
    public override string Name => "Обучалка";

    public override bool VisiblyDescription => false;

    public ProcessorHelp(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        _processors ??= _serviceProvider.GetServices<AbstractProcessor>().ToList();
        
        string answer = string.Join('\n',
            _processors.Where(processor => processor.VisiblyDescription)
                .Select(processor => $"{processor.Name}\n{processor.Description}\n")
        );
        bot.SendMessage(message.CreateResponse(answer));
    }
}