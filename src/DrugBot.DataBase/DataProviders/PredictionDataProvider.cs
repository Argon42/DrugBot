using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class PredictionDataProvider(PredictionDbContext dbContext) : IPredictionDataProvider
{
    public string GetRandomPrediction(int userId)
    {
        if (!dbContext.Predictions.Any())
        {
            return "Хьюстон, у нас проблемы!";
        }
        
        Random rnd = new(BotHandler.GetDayUserSeed(userId));
        
        var prediction = GetPrediction(rnd, dbContext);

        return prediction;
    }
    
    private static string GetPrediction(Random rnd, PredictionDbContext dbContext)
    {
        try
        {
            return dbContext.Predictions.ElementAt(new Random().Next(1, dbContext.Predictions.Count()) - 1).Prediction;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}