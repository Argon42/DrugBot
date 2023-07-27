namespace DrugBot.Core.Common;

public static class CommonExtensions
{
    public static List<T> ForEach<T>(this List<T> self, Action<T> action)
    {
        foreach (T item in self)
        {
            action(item);
        }

        return self;
    }

    public static T[] ForEach<T>(this T[] self, Action<T> action)
    {
        foreach (T item in self)
        {
            action(item);
        }

        return self;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        foreach (T item in self)
        {
            action(item);
        }

        return self;
    }
}