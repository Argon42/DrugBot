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
        if (!_dbContext.Anecdotes.Any()) 
        {
            return null;
        }
        
        var data = _dbContext.Anecdotes.ElementAt(new Random().Next(1, _dbContext.Anecdotes.Count()) - 1);

        return data;
    }

    public CommunityAnecdoteData? GetRandomAnecdoteFromUser(long userId)
    {
        var userRecords = _dbContext.Anecdotes.Where(x => x.UserId == userId);
        
        if (!userRecords.Any())
        {
            return null;
        }
        
        var data = userRecords.ElementAt(new Random().Next(1, userRecords.Count()) - 1);

        return data;
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