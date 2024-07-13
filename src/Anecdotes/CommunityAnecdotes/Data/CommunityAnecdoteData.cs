namespace Anecdotes.CommunityAnecdotes.Data;

public class CommunityAnecdoteData
{
    public int Id { get; internal set; }
    public ulong UserId { get; internal set; }
    public string Anecdote { get; internal set; } = string.Empty;
}