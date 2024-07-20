using DrugBot.DataBase.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugBot.DataBase.Initializers;

public class WisdomDbInitializer(
    ILogger<WisdomDbInitializer> logger,
    WisdomDbContext context)
{
    public async Task Initialize()
    {
        await CreateDb();
    }

    private async Task CreateDb()
    {
        try
        {
            await context.Database.EnsureCreatedAsync();

            if((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Wisdom database migration failed");
        }
    }
}