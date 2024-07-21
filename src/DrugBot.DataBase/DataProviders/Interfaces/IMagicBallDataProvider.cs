namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IMagicBallDataProvider : IArrayCount
{
    public string GetAnswer(int answerPosition);
}