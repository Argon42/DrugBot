using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class PredictionDataProvider(PredictionDbContext dbContext) : IPredictionDataProvider
{
    public string GetPrediction(int predictionPosition)
    {
        return dbContext.Predictions.ElementAt(predictionPosition).Prediction;
    }

    public int GetArrayCount()
    {
        return dbContext.Predictions.Count();
    }
}