using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class PredictionDataProvider(PredictionDbContext dbContext) : IPredictionDataProvider, IDbCount
{
    public string GetPrediction(int predictionPosition)
    {
        return dbContext.Predictions.ElementAt(new Random().Next(1, dbContext.Predictions.Count()) - 1).Prediction;
    }

    public int GetArrayCount()
    {
        return dbContext.Predictions.Count();
    }
}