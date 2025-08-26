using DrugBot;
using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

public class DiplomaResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        Random rnd = new(BotHandler.GetDayUserSeed(message.User.GetHashCode()));
        var pages = rnd.Next(-3, 130);
        var originality = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
        var chanceOfSurrender = rnd.Next(0, 100) + Math.Round(rnd.NextDouble(), 2);
        var prediction = GetPrediction(rnd);

        var result = $"[id{message.User}|Ваш] диплом состоит из {pages} страниц(ы) , \n" +
                     $"текущая оригинальность {originality}%, \n" +
                     $"шанс сдать = {chanceOfSurrender}%\n" +
                     $"Предсказание к диплому: {prediction}";
        bot.SendMessage(message.CreateResponse(result));
    }
    
    private static string GetPrediction(Random rnd)
    {
        try
        {
            const string path = "Local/predictions.txt";
            return BotHandler.GetRandomLineFromFile(rnd, path);
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}