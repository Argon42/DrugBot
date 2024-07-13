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
        var rawData = _dbContext.Anecdotes.FromSqlRaw("SELECT * FROM Anecdotes ORDER BY RANDOM() LIMIT 1");

        if (rawData == null || !rawData.Any())
        {
            return null;
        }
        
        var data = rawData.ToArray();

        return data[0];
    }

    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(ulong userId)
    {
        var rawData = _dbContext.Anecdotes.FromSqlRaw($"SELECT * FROM Anecdotes WHERE `User` = {userId}  ORDER BY RANDOM() LIMIT 1");

        if (!rawData.Any())
        {
            return null;
        }
        
        var data = rawData.ToArray();

        return data[0];
    }

    public void CreateNewAnecdote(ulong userId, string anecdote)
    {
        _dbContext.Anecdotes.Add(CommunityAnecdoteDataFactory.Create(userId, anecdote));
        _dbContext.SaveChanges();
    }
}