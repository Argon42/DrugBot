namespace CustomProcessors.Behaviours.Triggers;

[Serializable]
internal class MessageTextStartFrom : Trigger
{
    public string Message { get; init; } = null!;

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence) =>
        message.Text.ToLower().Trim().StartsWith(Message.ToLower().Trim());
}