using Anecdotes.CommunityAnecdotes.Data;

namespace Anecdotes.CommunityAnecdotes.Repositories.Interfaces;

public interface IAnecdoteRepository
{
    public CommulityAnecdoteData? GetRandomAnecdote();
    public CommulityAnecdoteData? GetRandomAnecdoteFromUser(ulong userId);
}