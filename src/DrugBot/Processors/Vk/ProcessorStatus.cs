﻿using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Exception;

namespace DrugBot.Processors;

[Processor]
public class ProcessorStatus : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/статус"
    };

    private readonly IFactory<IVkApi> _vkApi;

    public ProcessorStatus(IFactory<IVkApi> vkApi)
    {
        _vkApi = vkApi;
    }

    public override string Description =>
        $"Хочешь получить случайный статус участника, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Случайный статус";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        List<string> statuses;
        try
        {
            statuses = _vkApi.Create().Messages.GetConversationMembers(((IVkMessage)message).User.PeerId.Value, new[] { "status" })
                .Profiles
                .Where(p => !string.IsNullOrEmpty(p.Status))
                .Select(p => p.Status)
                .ToList();
        }
        catch (ConversationAccessDeniedException)
        {
            string error = "Для вывода случайного статуса участника, боту необходимы права администратора";
            bot.SendMessage(message.CreateResponse(error));
            return;
        }

        Random rnd = new();
        int result = rnd.Next(0, statuses.Count());
        string status = statuses[result];
        bot.SendMessage(message.CreateResponse(status));
    }
}