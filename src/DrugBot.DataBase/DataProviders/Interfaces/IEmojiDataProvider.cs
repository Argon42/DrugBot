namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IEmojiDataProvider
{
    public string GetEmoji(int emojiPosition);
    public int GetArrayCount();
}