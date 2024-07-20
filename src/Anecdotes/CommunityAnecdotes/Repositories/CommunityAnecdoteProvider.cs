using Anecdotes.CommunityAnecdotes.Data;
using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Repositories;

public class CommunityAnecdoteProvider : IAnecdoteProvider
{
    private readonly CommunityAnecdoteDbContext _dbContext;
    
    public CommunityAnecdoteProvider(CommunityAnecdoteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public CommunityAnecdoteData? GetRandomAnecdote()
    {
        var rawData = _dbContext.Anecdotes.FromSqlRaw("SELECT * FROM anecdotes ORDER BY RANDOM() LIMIT 1");

        if (rawData.Count() == 0) //Нельзя использовать !rawData.Any()
        {
            return null;
        }
        
        var data = rawData.ToArray();

        return data[0];
    }

    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(long userId)
    {
        var rawData =
            _dbContext.Anecdotes.FromSqlRaw(
                "SELECT * FROM anecdotes WHERE user_id IN ({0}) ORDER BY RANDOM() LIMIT 1", userId);

        if (rawData.Count() == 0) //Нельзя использовать !rawData.Any()
        {
            return null;
        }
        
        var data = rawData.ToArray();

        return data[0];
    }

    public void CreateNewAnecdote(long userId, string anecdote)
    {
        _dbContext.Anecdotes.Add(new CommunityAnecdoteData
        {
            UserId = userId,
            Anecdote = anecdote
        });
        _dbContext.SaveChanges();
    }
}