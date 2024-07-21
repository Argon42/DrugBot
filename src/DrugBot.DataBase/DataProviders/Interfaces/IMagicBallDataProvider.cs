namespace DrugBot.DataBase.DataProviders.Interfaces;

public interface IMagicBallDataProvider
{
    public string GetAnswer(int answerPosition);
    public int GetArrayCount();
}