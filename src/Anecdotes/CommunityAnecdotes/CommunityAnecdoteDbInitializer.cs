using Anecdotes.CommunityAnecdotes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Anecdotes.CommunityAnecdotes;

public class CommunityAnecdoteDbInitializer
{
    private readonly ILogger<CommunityAnecdoteDbInitializer> _logger;
    private readonly CommunityAnecdoteDbContext _context;

    public CommunityAnecdoteDbInitializer(
        ILogger<CommunityAnecdoteDbInitializer> logger,
        CommunityAnecdoteDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Initialize()
    {
        await CreateDb();
    }

    private async Task CreateDb()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();

            if((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Database migration failed");
        }
    }
}