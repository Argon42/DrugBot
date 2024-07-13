using Anecdotes.CommunityAnecdotes.Data;
using Anecdotes.CommunityAnecdotes.Factories;
using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Repositories;

public class CommunityAnecdoteController : IAnecdoteController
{
    private readonly CommunityAnecdoteDbContext _dbContext;
    
    public CommunityAnecdoteController(CommunityAnecdoteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public CommunityAnecdoteData? GetRandomAnecdote()
    {
        var rawData = _dbContext.Anecdotes.FromSqlRaw("SELECT * FROM anecdotes");

        if (rawData == null || rawData.Count() == 0) //Нельзя использовать !rawData.Any() вместо rawData.Count() == 0
        {
            return null;
        }
        
        var data = rawData.ToArray();

        return data[0];
    }

    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(ulong userId)
    {
        var rawData = _dbContext.Anecdotes.FromSqlRaw("SELECT * FROM Anecdotes WHERE `User` = {0}  ORDER BY RANDOM() LIMIT 1", userId);

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