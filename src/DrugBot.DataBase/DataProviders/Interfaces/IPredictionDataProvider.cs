namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IPredictionDataProvider
{
    public string GetPrediction(int predictionPosition);
    public int GetArrayCount();
}