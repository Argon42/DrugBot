using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class MagicBallDataProvider(MagicBallDbContext dbContext) : IMagicBallDataProvider, IDbCount
{
    public string GetAnswer(int answerPosition)
    {
        return dbContext.Answers.ElementAt(answerPosition).Answer;
    }

    public int GetArrayCount()
    {
        return dbContext.Answers.Count();
    }
}