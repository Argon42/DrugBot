using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorPrediction : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/8",
        "!8"
    };

    public override string Description =>
        $"Хочешь получить ответ на вопрос, напиши: {string.Join(" | ", keys)} вопрос";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Волшебный шар";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        string path = "Local/8.txt";

        int seed = new Random().Next(int.MinValue, int.MaxValue);
        if (sentence.Length > 1)
            seed = BotHandler.GetDayUserSeed(message.PeerId) + message.Text.GetHashCode();

        BotHandler.SendMessage(vkApi, message.PeerId, BotHandler.GetRandomLineFromFile(new Random(seed), path), message);
    }
}