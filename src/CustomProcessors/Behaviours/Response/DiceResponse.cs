using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class DiceResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var sentence = message.Text.Split();
        var dices = sentence[1].Split('d');
        if (!int.TryParse(dices[0], out var diceCount)) return;
        if (!int.TryParse(dices[1], out var diceValue)) return;
        if (diceValue < 0 || diceCount < 0) return;

        var modificator = 0;
        if (sentence.Length >= 3) int.TryParse(sentence[2], out modificator);

        var rnd = new Random();

        var result = 0;
        for (var i = 0; i < diceCount; i++)
            result += rnd.Next(0, diceValue) + 1;

        bot.SendMessage(message.CreateResponse($"Выпало {result} + {modificator} = {result + modificator}"));
    }
}