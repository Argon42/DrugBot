using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DrugBot.Bot;
using DrugBot.Core;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DrugBot.Processors;

[Processor]
public class ProcessorHelp : AbstractProcessor
{
    private IReadOnlyList<IProcessor>? _processors;

    private readonly List<string> keys = new()
    {
        "/help",
        "!help",
        "/помощь",
        "!помощь",
    };

    private readonly IServiceProvider _serviceProvider;

    public override string Description => "тутор со всеми командами";
    public override IReadOnlyList<string> Keys => keys;
    public override string Name => "Обучалка";

    public override bool VisiblyDescription => false;

    public ProcessorHelp(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        _processors ??= _serviceProvider.GetServices<IProcessor>().ToList();

        string answer = string.Join('\n',
            _processors.Where(processor => processor.VisiblyDescription)
                .Select(processor => $"{processor.Name}\n{processor.Description}\n")
        );
        bot.SendMessage(message.CreateResponse(answer));
    }
}