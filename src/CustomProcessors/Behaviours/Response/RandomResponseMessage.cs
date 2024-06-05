using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

[Serializable]
internal class RandomResponseMessage : SendResponseMessage
{
    public string[] Messages { get; set; } = Array.Empty<string>();
    public string PathToFile { get; init; } = string.Empty;
    public RandomFunction RandomFunction { get; init; } = new();

    public override void ProcessMessage<TUser, TMessage>(
        IBot<TUser, TMessage> bot,
        TMessage message,
        CancellationToken token)
    {
        if (Messages.Length == 0 && PathToFile != string.Empty)
        {
            Messages = File.ReadAllLines(PathToFile);
        }

        if (Messages.Length <= 0)
            return;

        var index = RandomFunction.GetRandomValue(0, Messages.Length, message, message.User, 0);
        var resultMessage = Messages[index];
        bot.SendMessage(message.CreateResponse(resultMessage));
    }
}