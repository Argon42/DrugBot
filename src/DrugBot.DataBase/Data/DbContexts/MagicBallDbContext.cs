using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class MagicBallDbContext(DbContextOptions<MagicBallDbContext> options) : DbContext(options)
{
    public DbSet<MagicBallData> Answers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildEmojis(modelBuilder);
    }

    private void BuildEmojis(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<MagicBallData>();

        configuration.HasData(new MagicBallData{ Id = 1, Answer = "ДА"});
        configuration.HasData(new MagicBallData{ Id = 2, Answer = "очень вероятно"});
        configuration.HasData(new MagicBallData{ Id = 3, Answer = "безусловно"});
        configuration.HasData(new MagicBallData{ Id = 4, Answer = "без сомнений"});
        configuration.HasData(new MagicBallData{ Id = 5, Answer = "должно быть так"});
        configuration.HasData(new MagicBallData{ Id = 6, Answer = "абсолютно точно"});
        configuration.HasData(new MagicBallData{ Id = 7, Answer = "мне кажется да"});
        configuration.HasData(new MagicBallData{ Id = 8, Answer = "духи говорят да"});
        configuration.HasData(new MagicBallData{ Id = 9, Answer = "похоже, что да"});
        configuration.HasData(new MagicBallData{ Id = 10, Answer = "НЕТ"});
    }
}