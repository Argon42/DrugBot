using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Data;

public class CommunityAnecdoteDbContext : DbContext
{
    public DbSet<CommulityAnecdoteData> Anecdotes;

    public CommunityAnecdoteDbContext(DbContextOptions<CommunityAnecdoteDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        BuildAnecdotes(modelBuilder);
    }

    private void BuildAnecdotes(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<CommulityAnecdoteData>();
    }
}