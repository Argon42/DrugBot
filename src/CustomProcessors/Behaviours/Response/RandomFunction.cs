using DrugBot.Core.Bot;

namespace CustomProcessors.Behaviours.Response;

[Serializable]
internal class RandomFunction
{
    public bool IsRandom { get; init; }
    public bool IsRandomByUser { get; init; }
    public bool IsRandomByDay { get; init; }
    public bool IsRandomByMessage { get; init; }

    public int GetRandomValue(int from, int to, IMessage message, IUser user, int index)
    {
        var seed = GetSeed(index, user.Id, message.Text);
        var rnd = new Random(seed);
        return rnd.Next(from, to);
    }

    private int GetSeed(int index, long fromId, string messageText)
    {
        var seed = IsRandom && !(IsRandomByUser || IsRandomByDay || IsRandomByMessage)
            ? (int)DateTime.Now.Ticks ^ 781267823
            : 0;

        if (IsRandomByUser)
        {
            seed += GetHash(fromId);
        }

        if (IsRandomByDay)
        {
            seed += GetHash(DateTime.Today);
        }

        if (IsRandomByMessage)
        {
            seed += GetHash(messageText);
        }

        return seed + GetHash(index);
    }

    private static int GetHash(object start)
    {
        var hash = start.GetHashCode();
        var countOfHashing = start.GetHashCode();

        for (var i = 0; i < Math.Abs(countOfHashing) % 10; i++)
            hash = hash.GetHashCode();

        return hash;
    }
}