namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IEmojiDataProvider : IArrayCount
{
    public string GetEmoji(int emojiPosition);
}