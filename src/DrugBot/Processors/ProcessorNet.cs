using System;
using System.Collections.Generic;
using DrugBot.Bot;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorNet : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "нет",
    };

    public override string Description => "Напиши \"Нет\"";
    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Нет";

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence) =>
        string.Equals(message.Text.TrimEnd(), "нет", StringComparison.CurrentCultureIgnoreCase);

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        bot.SendMessage(message.CreateResponse("Пидора ответ"));
    }
}