using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Data;

public class CommunityAnecdoteDbContext : DbContext
{
    public DbSet<CommunityAnecdoteData> Anecdotes { get; init; }
    
    public CommunityAnecdoteDbContext(DbContextOptions<CommunityAnecdoteDbContext> options) : base(options) { }
}