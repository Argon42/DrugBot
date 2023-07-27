using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DrugBot.Bot.Vk;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using VkNet.Abstractions;
using VkNet.Exception;

namespace DrugBot.Processors.Vk;

[Processor]
public class ProcessorWho : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/who",
        "/кто",
    };

    private readonly IFactory<IVkApi> _factory;

    public override string Description =>
        $"Бот знает все обо всех, для вызова используйте {string.Join(' ', _keys)} вопрос";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Кто?";

    public ProcessorWho(IFactory<IVkApi> factory) => _factory = factory;

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence)
    {
        return message is IVkMessage && sentence.Length > 0 &&
               _keys.Any(s => sentence[0].Equals(s, StringComparison.CurrentCultureIgnoreCase));
    }

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        List<string> names;
        try
        {
            names = _factory.Create().Messages
                .GetConversationMembers(((IVkMessage)message).User.PeerId ?? -1, new[] { "" })
                .Profiles
                .Select(user => $"{user.FirstName} {user.LastName}")
                .ToList();
        }
        catch (ConversationAccessDeniedException)
        {
            string s = "Для вывода случайного статуса участника, боту необходимы права администратора";
            bot.SendMessage(message.CreateResponse(s));
            return;
        }

        string question = message.Text.Substring(message.Text.Split()[0].Length).TrimStart()
            .TrimEnd("?!".ToCharArray());
        Random rnd = new(question.ToLower().GetHashCode());
        int result = rnd.Next(0, names.Count());
        double chanceOfNothing = rnd.NextDouble();
        string answer = $"{(chanceOfNothing > 0.9 ? "Никто не" : names[result])} {question}";
        bot.SendMessage(message.CreateResponse(answer));
    }
}