using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class WisdomDataProvider(WisdomDbContext dbContext) : IWisdomDataProvider
{
    public string GetWisdom(int wisdomPosition)
    {
        return dbContext.Wisdoms.ElementAt(wisdomPosition).Wisdom;
    }

    public int GetArrayCount()
    {
        return dbContext.Wisdoms.Count();
    }
}