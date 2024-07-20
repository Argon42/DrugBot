using Anecdotes.CommunityAnecdotes.Data;

namespace Anecdotes.CommunityAnecdotes.Factories;

internal static class CommunityAnecdoteDataFactory
{
    internal static CommunityAnecdoteData Create(long userId, string anecdote) => new()
    {
        UserId = userId,
        Anecdote = anecdote
    };
}