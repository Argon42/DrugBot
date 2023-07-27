using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Triggers;

public abstract class Trigger : BaseBehaviour
{
    public abstract bool HasTrigger<TMessage>(TMessage message, string[] sentence) where TMessage : IMessage;
}