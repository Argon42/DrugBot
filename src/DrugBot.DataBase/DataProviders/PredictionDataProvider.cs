using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class PredictionDataProvider(PredictionDbContext dbContext) : IPredictionDataProvider
{
    public string GetRandomPrediction()
    {
        return !dbContext.Predictions.Any()
            ? "Хьюстон, у нас проблемы!"
            : dbContext.Predictions.ElementAt(new Random().Next(1, dbContext.Predictions.Count()) - 1).Prediction;
    }
}