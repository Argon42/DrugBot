using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class MagicBallDbContext(DbContextOptions<MagicBallDbContext> options) : DbContext(options)
{
    public DbSet<MagicBallData> Answers { get; set; }
}