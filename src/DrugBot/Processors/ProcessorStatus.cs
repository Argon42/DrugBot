using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Exception;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorStatus : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/статус"
    };

    public override string Description =>
        $"Хочешь получить случайный статус участника, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Случайный статус";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        List<string> statuses;
        try
        {
            statuses = vkApi.Messages.GetConversationMembers(message.PeerId.Value, new[] { "status" })
                .Profiles
                .Where(p => !string.IsNullOrEmpty(p.Status))
                .Select(p => p.Status)
                .ToList();
        }
        catch (ConversationAccessDeniedException)
        {
            BotHandler.SendMessage(vkApi, message.PeerId,
                "Для вывода случайного статуса участника, боту необходимы права администратора");
            return;
        }

        Random rnd = new();
        int result = rnd.Next(0, statuses.Count());
        BotHandler.SendMessage(vkApi, message.PeerId, statuses[result]);
    }
}