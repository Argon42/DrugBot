using Anecdotes.CommunityAnecdotes.Data;

namespace Anecdotes.CommunityAnecdotes.Factories;

public static class CommunityAnecdoteDataFactory
{
    public static CommulityAnecdoteData Create(ulong userId, string anecdote) => new()
    {
        UserId = userId,
        Anecdote = anecdote
    };
}