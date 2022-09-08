using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorTry : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/try",
        "/попробовать",
        "/монетка"
    };

    public override string Description =>
        $"Если нужно бросить или попробовать что то сделать, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Пробователь";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        Random rnd = new();
        int result = rnd.Next();
        BotHandler.SendMessage(vkApi, message.PeerId, result % 2 == 0 ? "Успех" : "Провал");
    }
}