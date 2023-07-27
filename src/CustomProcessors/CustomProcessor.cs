using CustomProcessors.Behaviours.Response;
using CustomProcessors.Behaviours.Triggers;
using DrugBot.Core;
using DrugBot.Core.Bot;
using Microsoft.Extensions.Logging;

namespace CustomProcessors;

[Serializable]
public class CustomProcessor : IProcessor
{
    private readonly ILogger<CustomProcessor> _logger;
    public string Description { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Response Response { get; set; } = null!;
    public Trigger Trigger { get; set; } = null!;
    public bool VisiblyDescription { get; set; }

    public CustomProcessor(ILogger<CustomProcessor> logger) => _logger = logger;

    public bool HasTrigger<TMessage>(TMessage message, string[] sentence) where TMessage : IMessage =>
        Trigger.HasTrigger(message, sentence);

    public bool TryProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
        where TUser : IUser where TMessage : IMessage<TMessage, TUser>
    {
        try
        {
            Response.ProcessMessage(bot, message, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing {Name}", Name);
            return false;
        }

        return true;
    }
}