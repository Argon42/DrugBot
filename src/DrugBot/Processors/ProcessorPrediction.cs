﻿using System;
using System.Collections.Generic;
using System.Threading;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;

namespace DrugBot.Processors;

[Processor]
public class ProcessorPrediction : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/8",
        "!8",
    };

    public override string Description =>
        $"Хочешь получить ответ на вопрос, напиши: {string.Join(" | ", _keys)} вопрос";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Волшебный шар";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        string path = "Local/8.txt";

        int seed = new Random().Next(int.MinValue, int.MaxValue);
        if (message.Text.Split().Length > 1)
            seed = BotHandler.GetDayUserSeed(message.User.GetHashCode()) + message.Text.GetHashCode();

        string fromFile = BotHandler.GetRandomLineFromFile(new Random(seed), path);
        bot.SendMessage(message.CreateResponse(fromFile));
    }
}