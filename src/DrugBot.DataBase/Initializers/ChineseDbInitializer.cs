using DrugBot.DataBase.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugBot.DataBase.Initializers;

public class ChineseDbInitializer(
    ILogger<ChineseDbInitializer> logger,
    ChineseDbContext context)
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
            logger.LogError(e, "Chinese database migration failed");
        }
    }
}