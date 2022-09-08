using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorQuote : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/фраза"
    };

    public override string Description =>
        $"Спроси у бота посредственную фразу, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Рандомная цитата/фраза";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        string path = "Local/wisdom.txt";
        Random random = new();
        BotHandler.SendMessage(vkApi, message.PeerId, BotHandler.GetRandomLineFromFile(random, path), message);
    }
}