using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class ChineseDbContext(DbContextOptions<ChineseDbContext> options) : DbContext(options)
{
    public DbSet<ChineseData> ChineseSymbols { get; set; }
}