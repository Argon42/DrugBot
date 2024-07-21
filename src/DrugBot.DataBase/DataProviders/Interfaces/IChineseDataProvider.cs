namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IChineseDataProvider : IArrayCount
{
    public string GetChineseSymbol(int symbolPosition);
}