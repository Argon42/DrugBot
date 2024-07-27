namespace CustomProcessors.Behaviours.Triggers;

[Serializable]
internal class MessageTextStartFrom : Trigger
{
    public string[] Messages { get; init; } = null!;

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence)
    {
        var parsedMessage = message.Text.ToLower().Trim();

        return Messages.Any(s => parsedMessage.StartsWith(s.ToLower().Trim()));
    }
}