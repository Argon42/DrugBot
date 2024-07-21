using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.Processors;

[Processor]
public class ProcessorDeadChinese : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/мудрец",
    };

    public override string Description =>
        $"Пророчество от мудрого, но мертвого китайца, для вызова деда используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Мертвый китаец";

    private readonly IChineseDataProvider _chineseDataProvider;

    public ProcessorDeadChinese(IChineseDataProvider chineseDataProvider)
    {
        _chineseDataProvider = chineseDataProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));

        int predictionLength = (int)Math.Abs(15 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * rnd.NextDouble() - 1, 5)));
        StringBuilder stringBuilder = new($"Мудрец видит что в будущем будет {GetPrediction(rnd, predictionLength)}");

        bot.SendMessage(message.CreateResponse(stringBuilder.ToString()));
    }

    private string GetPrediction(Random rnd, int count)
    {
        try
        {
            var arrayCount = _chineseDataProvider.GetArrayCount();
            var builder = new StringBuilder();
            
            for (var i = 0; i < count; i++)
                builder.AppendJoin(' ', _chineseDataProvider.GetChineseSymbol(rnd.Next(0, arrayCount - 1)));
            //TODO Оптимизировать запрос: если пользователь уже набирал команду сегодня, выдавать сохраненный результат из отдельной таблицы
            
            return builder.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}