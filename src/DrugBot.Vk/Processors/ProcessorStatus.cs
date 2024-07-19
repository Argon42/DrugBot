using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using DrugBot.Vk.Bot;
using VkNet.Abstractions;
using VkNet.Exception;

namespace DrugBot.Vk.Processors;

[Processor]
public class ProcessorStatus : VkProcessor
{
    private readonly List<string> _keys = new()
    {
        "/статус",
    };

    public override string Description =>
        $"Хочешь получить случайный статус участника, для вызова используйте {string.Join(' ', _keys)}";

    public override IReadOnlyList<string> Keys => _keys;

    public override string Name => "Случайный статус";

    public ProcessorStatus(IVkApi api) : base(api) { }

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence) =>
        message is IVkMessage && base.HasTrigger(message, sentence);

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
    {
        List<string> statuses;
        try
        {
            statuses = Api.Messages
                .GetConversationMembers(((IVkMessage)message).User.PeerId.Value, new[] { "status" })
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