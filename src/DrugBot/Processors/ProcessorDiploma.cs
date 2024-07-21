using System;
using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.Processors;

[Processor]
public class ProcessorDiploma : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/диплом",
    };

    public override string Description =>
        $"Экстрасенсорный анализ диплома и бонус, для вызова используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "дИПЛОМОЗИТОР";

    private readonly IPredictionDataProvider _predictionDataProvider;

    public ProcessorDiploma(IPredictionDataProvider predictionDataProvider)
    {
        _predictionDataProvider = predictionDataProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
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

    private string GetPrediction(Random rnd)
    {
        try
        {
            var arrayCount = _predictionDataProvider.GetArrayCount();
            var prediction = _predictionDataProvider.GetPrediction(rnd.Next(0, arrayCount - 1));
            
            return prediction;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}