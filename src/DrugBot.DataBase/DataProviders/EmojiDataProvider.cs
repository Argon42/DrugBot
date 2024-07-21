using DrugBot.DataBase.Data.DbContexts;
using DrugBot.DataBase.DataProviders.Interfaces;

namespace DrugBot.DataBase.DataProviders;

public class EmojiDataProvider(EmojiDbContext dbContext) : IEmojiDataProvider
{
    public string GetEmoji(int emojiPosition)
    {
        return dbContext.Emojis.ElementAt(emojiPosition).Emoji;
    }

    public int GetArrayCount()
    {
        return dbContext.Emojis.Count();
    }
}