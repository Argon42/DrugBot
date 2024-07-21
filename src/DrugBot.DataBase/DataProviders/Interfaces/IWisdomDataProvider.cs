namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IWisdomDataProvider : IArrayCount
{
    public string GetWisdom(int wisdomPosition);
}