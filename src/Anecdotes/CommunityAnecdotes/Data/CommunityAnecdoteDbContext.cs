using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Data;

public class CommunityAnecdoteDbContext : DbContext
{
    internal DbSet<CommunityAnecdoteData> Anecdotes { get; init; }

    public CommunityAnecdoteDbContext(DbContextOptions<CommunityAnecdoteDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        BuildAnecdotes(modelBuilder);
    }

    private void BuildAnecdotes(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<CommunityAnecdoteData>();
    }
}