using Microsoft.EntityFrameworkCore;

namespace Anecdotes.CommunityAnecdotes.Data;

public class CommunityAnecdoteDbContext : DbContext
{
    public DbSet<CommunityAnecdoteData> Anecdotes { get; init; }

    private readonly string _connectionString;
    
    public CommunityAnecdoteDbContext(DbContextOptions<CommunityAnecdoteDbContext> options) : base(options) { }

    public CommunityAnecdoteDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        BuildAnecdotes(modelBuilder);
    }

    private void BuildAnecdotes(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<CommunityAnecdoteData>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(_connectionString).UseSnakeCaseNamingConvention();
    }
}