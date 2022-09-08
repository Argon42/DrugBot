using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorBibasiks : AbstractProcessor
{
    private readonly List<string> keys = new()
    {
        "/бибасики"
    };

    public override string Description =>
        $"Узнай размеры своих бибасиков, для вызова используйте {string.Join(' ', keys)}";

    public override IReadOnlyList<string> Keys => keys;

    public override string Name => "Бибасикометр";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        try
        {
            if (message.FromId != null &&
                vkApi.Users.Get(new[] { message.FromId.Value }, ProfileFields.Sex)[0].Sex ==
                Sex.Male)
            {
                BotHandler.SendMessage(vkApi, message.PeerId,
                    new Random().NextDouble() > 0.5
                        ? "У вас нет бибасиков у только вас биба"
                        : "Бибасики только для девушек, у вас только биба");
                return;
            }
        }
        catch (Exception)
        {
            // ignored
        }

        Random rnd = new(BotHandler.GetDayUserSeed(message.FromId));

        float length = (float)rnd.NextDouble();
        double resultLenght = 80 + Math.Tan(0.5 * Math.PI * (2 * length - 1));

        BotHandler.SendMessage(vkApi, message.PeerId, $"Сегодня ваши бибасики {resultLenght:F1} см в обхвате");
    }
}