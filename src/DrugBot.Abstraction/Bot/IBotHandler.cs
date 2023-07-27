namespace DrugBot.Core.Bot;

public interface IBotHandler
{
    bool IsWork { get; }
    string Name { get; }

    void Initialize();
    void Start();
    void Stop();
}