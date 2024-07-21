using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class EmojiDbContext(DbContextOptions<EmojiDbContext> options) : DbContext(options)
{
    public DbSet<EmojiData> Emojis { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildEmojis(modelBuilder);
    }

    private void BuildEmojis(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<EmojiData>();

        configuration.HasData(new EmojiData{ Id = 1, Emoji = "&#128512;"});
        configuration.HasData(new EmojiData{ Id = 2, Emoji = "&#128515;"});
        configuration.HasData(new EmojiData{ Id = 3, Emoji = "&#128516;"});
        configuration.HasData(new EmojiData{ Id = 4, Emoji = "&#128513;"});
        configuration.HasData(new EmojiData{ Id = 5, Emoji = "&#128518;"});
        configuration.HasData(new EmojiData{ Id = 6, Emoji = "&#128517;"});
        configuration.HasData(new EmojiData{ Id = 7, Emoji = "&#129315;"});
        configuration.HasData(new EmojiData{ Id = 8, Emoji = "&#128514;"});
        configuration.HasData(new EmojiData{ Id = 9, Emoji = "&#128578;"});
        configuration.HasData(new EmojiData{ Id = 10, Emoji = "&#128579;"});
    }
}