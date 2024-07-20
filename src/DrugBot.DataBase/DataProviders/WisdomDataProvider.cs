using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class WisdomDataProvider(WisdomDbContext dbContext) : IWisdomDataProvider
{
    public string GetRandomWisdom(int userId)
    {
        if (!dbContext.Wisdoms.Any())
        {
            return "Хьюстон, у нас проблемы!";
        }
        
        return dbContext.Wisdoms.ElementAt(new Random().Next(1, dbContext.Wisdoms.Count()) - 1).Wisdom;
    }
}