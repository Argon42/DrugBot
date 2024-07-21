namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IWisdomDataProvider
{
    public string GetWisdom(int wisdomPosition);
    public int GetArrayCount();
}