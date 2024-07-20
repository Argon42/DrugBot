using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class EmojiDbContext(DbContextOptions<EmojiDbContext> options) : DbContext(options)
{
    public DbSet<EmojiData> Emojis { get; set; }
}