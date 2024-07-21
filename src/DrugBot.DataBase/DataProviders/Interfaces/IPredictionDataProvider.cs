namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IPredictionDataProvider : IArrayCount
{
    public string GetPrediction(int predictionPosition);
}