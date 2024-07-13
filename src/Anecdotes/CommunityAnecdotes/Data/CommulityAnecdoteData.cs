namespace Anecdotes.CommunityAnecdotes.Data;

public class CommulityAnecdoteData
{
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public string Anecdote { get; set; } = string.Empty;
}