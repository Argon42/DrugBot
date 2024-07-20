using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class MagicBallDataProvider(MagicBallDbContext dbContext) : IMagicBallDataProvider
{
    public string GetRandomPrediction()
    {
        return !dbContext.Answers.Any()
            ? "Хьюстон, у нас проблемы!"
            : dbContext.Answers.ElementAt(new Random().Next(1, dbContext.Answers.Count()) - 1).Prediction;
    }
}