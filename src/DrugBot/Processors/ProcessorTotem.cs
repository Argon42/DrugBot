using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorTotem : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/тотем"
    };

    public override string Description =>
        $"Посмотреть за ширму вселенной и узнать что тебя сегодня ждет, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Тотем дня";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.FromId));


        StringBuilder stringBuilder = new($"Сегодня вас ждет {GetPrediction(rnd, rnd.Next(3, 6))}");

        BotHandler.SendMessage(vkApi, message.PeerId, stringBuilder.ToString());
    }

    private static string GetPrediction(Random rnd, int count)
    {
        try
        {
            string path = "Local/emodzy.txt";
            return string.Join(' ', BotHandler.GetRandomLineFromFile(rnd, path, count));
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}