using Anecdotes.CommunityAnecdotes.Data;

namespace Anecdotes.CommunityAnecdotes.Repositories.Interfaces;

public interface IAnecdoteController
{
    public CommunityAnecdoteData? GetRandomAnecdote();
    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(long userId);
    public void CreateNewAnecdote(long userId, string anecdote);
}