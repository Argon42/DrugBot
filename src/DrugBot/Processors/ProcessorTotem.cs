using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.Processors;

[Processor]
public class ProcessorTotem : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/тотем",
    };

    public override string Description =>
        $"Посмотреть за ширму вселенной и узнать что тебя сегодня ждет, для вызова используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Тотем дня";

    private readonly IEmojiDataProvider _emojiDataProvider;

    public ProcessorTotem(IEmojiDataProvider emojiDataProvider)
    {
        _emojiDataProvider = emojiDataProvider;
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));
        StringBuilder stringBuilder = new($"Сегодня вас ждет {GetPrediction(rnd, rnd.Next(3, 6))}");

        bot.SendMessage(message.CreateResponse(stringBuilder.ToString()));
    }

    private string GetPrediction(Random rnd, int count)
    {
        try
        {
            var arrayCount = _emojiDataProvider.GetArrayCount();
            var emojis = new List<string>();

            for (var i = 0; i < count; i++)
            {
                emojis.Add(_emojiDataProvider.GetEmoji(rnd.Next(0, arrayCount - 1)));
            }
            
            return string.Join(' ', emojis);
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}