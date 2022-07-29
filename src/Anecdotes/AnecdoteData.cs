namespace Anecdotes;

public class AnecdoteData
{
    public string Anecdote { get; }
    public int AnecdoteNumber { get; }
    public string Page { get; }

    public AnecdoteData(string page, int anecdoteNumber, string anecdote)
    {
        Page = page;
        AnecdoteNumber = anecdoteNumber;
        Anecdote = anecdote;
    }
}