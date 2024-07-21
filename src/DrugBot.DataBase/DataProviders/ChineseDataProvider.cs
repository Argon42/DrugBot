using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class ChineseDataProvider(ChineseDbContext dbContext) : IChineseDataProvider, IDbCount
{
    public string GetChineseSymbol(int symbolPosition)
    {
        return dbContext.ChineseSymbols.ElementAt(symbolPosition).ChineseSymbol.ToString();
    }

    public int GetArrayCount()
    {
        return dbContext.ChineseSymbols.Count();
    }
}