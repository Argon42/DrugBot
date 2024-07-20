using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class WisdomDbContext(DbContextOptions<WisdomDbContext> options) : DbContext(options)
{
    public DbSet<WisdomData> Wisdoms { get; set; }
}