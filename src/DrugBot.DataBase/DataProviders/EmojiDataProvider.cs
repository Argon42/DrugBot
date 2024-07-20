using System.Text;
using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class EmojiDataProvider(EmojiDbContext dbContext) : IEmojiDataProvider
{
    public string GetRandomEmojis(int userId)
    {
        var  rnd = new Random(BotHandler.GetDayUserSeed(userId));
        var result = $"Сегодня вас ждет {GetPrediction(rnd, rnd.Next(3, 6), dbContext)}";

        return result;
    }
    
    private static string GetPrediction(Random rnd, int count, EmojiDbContext dbContext)
    {
        try
        {
            var builder = new StringBuilder();

            for (var i = 0; i < count; i++)
                builder.AppendJoin(' ', dbContext.Emojis.ElementAt(rnd.Next(1, dbContext.Emojis.Count()) - 1).Emoji);

            return builder.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}