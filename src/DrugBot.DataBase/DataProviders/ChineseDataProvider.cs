using System.Text;
using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class ChineseDataProvider(ChineseDbContext dbContext) : IChineseDataProvider
{
    public string GetRandomChineseString(int userId)
    {
        if (!dbContext.ChineseSymbols.Any())
        {
            return "Хьюстон, у нас проблемы!";
        }
        
        Random rnd = new(BotHandler.GetDayUserSeed(userId));

        var predictionLength = (int)Math.Abs(15 + Math.Tan(0.5 * Math.PI * Math.Pow(2 * rnd.NextDouble() - 1, 5)));
        var result = $"Мудрец видит что в будущем будет {GetPrediction(rnd, predictionLength, dbContext)}";

        return result;
    }
    
    private static string GetPrediction(Random rnd, int count, ChineseDbContext dbContext)
    {
        try
        {
            StringBuilder builder = new();

            for (var i = 0; i < count; i++)
                builder.AppendJoin(' ',
                    dbContext.ChineseSymbols.ElementAt(rnd.Next(1, dbContext.ChineseSymbols.Count()) - 1)
                        .ChineseSymbol);
            
            return builder.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}