namespace CustomProcessors.Behaviours.Triggers;

[Serializable]
internal class MessageTextStartFrom : Trigger
{
    public string[] Messages { get; init; } = null!;

    public override bool HasTrigger<TMessage>(TMessage message, string[] sentence)
    {
        var parsedMessage = message.Text.Split(' ')[0].ToLower().Trim();

        return Messages.Any(s => parsedMessage == s.ToLower().Trim());
    }
}