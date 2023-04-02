using System;
using System.Collections.Generic;
using DrugBot.Bot;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorDiploma : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/диплом",
    };

    public override string Description =>
        $"Экстрасенсорный анализ диплома и бонус, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "дИПЛОМОЗИТОР";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));
        int pages = rnd.Next(-3, 130);
        double originality = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
        double chanceOfSurrender = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
        string prediction = GetPrediction(rnd);

        string result = $"[id{message.User}|Ваш] диплом состоит из {pages} страниц(ы) , \n" +
                        $"текущая оригинальность {originality}%, \n" +
                        $"шанс сдать = {chanceOfSurrender}%\n" +
                        $"Предсказание к диплому: {prediction}";
        bot.SendMessage(message.CreateResponse(result));
    }

    private static string GetPrediction(Random rnd)
    {
        try
        {
            string path = "Local/predictions.txt";
            return BotHandler.GetRandomLineFromFile(rnd, path);
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}