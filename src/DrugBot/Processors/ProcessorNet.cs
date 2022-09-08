using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorNet : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "нет"
    };

    public override string Description => "Напиши \"Нет\"";
    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Нет";

    public override bool HasTrigger(Message message, string[] sentence)
    {
        return string.Equals(message.Text.TrimEnd(), "нет", StringComparison.CurrentCultureIgnoreCase);
    }

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        BotHandler.SendMessage(vkApi, message.PeerId, "Пидора ответ");
    }
}