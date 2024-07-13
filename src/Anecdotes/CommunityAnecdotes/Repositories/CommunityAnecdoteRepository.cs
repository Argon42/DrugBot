using Anecdotes.CommunityAnecdotes.Data;
using Anecdotes.CommunityAnecdotes.Factories;
using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Repositories;

public class CommunityAnecdoteRepository : IAnecdoteRepository
{
    private readonly CommunityAnecdoteDbContext _dbContext;

    public CommunityAnecdoteRepository(CommunityAnecdoteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public CommunityAnecdoteData? GetRandomAnecdote()
    {
        var data = _dbContext.Anecdotes.FromSqlRaw("SELECT * FROM Anecdotes ORDER BY RANDOM() LIMIT 1").ToArray();

        return data.Length == 0 ? null : data[0];
    }

    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(ulong userId)
    {
        var data = _dbContext.Anecdotes.FromSqlRaw($"SELECT * FROM Anecdotes WHERE `User` = {userId}  ORDER BY RANDOM() LIMIT 1").ToArray();

        return data.Length == 0 ? null : data[0];
    }

    public void CreateNewAnecdote(ulong userId, string anecdote)
    {
        _dbContext.Anecdotes.Add(CommunityAnecdoteDataFactory.Create(userId, anecdote));
        _dbContext.SaveChanges();
    }
}