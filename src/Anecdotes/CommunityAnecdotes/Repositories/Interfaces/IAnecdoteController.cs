using Anecdotes.CommunityAnecdotes.Data;

namespace Anecdotes.CommunityAnecdotes.Repositories.Interfaces;

public interface IAnecdoteController
{
    public CommunityAnecdoteData? GetRandomAnecdote();
    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(ulong userId);
    public void CreateNewAnecdote(ulong userId, string anecdote);
}