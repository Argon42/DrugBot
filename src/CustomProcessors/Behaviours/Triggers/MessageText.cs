namespace CustomProcessors.Behaviours.Triggers;

[Serializable]
internal class MessageText : Trigger
{
    public string Message { get; init; } = null!;

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence)
    {
        return message.Text.ToLower().Trim() == Message.ToLower().Trim();
    }
}