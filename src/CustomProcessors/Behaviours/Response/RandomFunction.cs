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
            seed ^= (int)(fromId >> 32);
        }

        if (IsRandomByDay)
        {
            seed ^= DateTime.Today.GetHashCode();
        }

        if (IsRandomByMessage)
        {
            seed ^= Fnv1A32Hash(messageText);
        }

        seed ^= index;
        return seed;
    }

    private static int Fnv1A32Hash(string str)
    {
        const int fnvOffsetBasis = unchecked((int)2166136261);
        const int fnvPrime = 16777619;

        var hash = fnvOffsetBasis;

        foreach (var c in str)
        {
            hash ^= c;
            hash *= fnvPrime;
        }

        return hash;
    }
}