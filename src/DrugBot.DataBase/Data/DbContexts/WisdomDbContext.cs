using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class WisdomDbContext(DbContextOptions<WisdomDbContext> options) : DbContext(options)
{
    public DbSet<WisdomData> Wisdoms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildEmojis(modelBuilder);
    }

    private void BuildEmojis(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<WisdomData>();

        configuration.HasData(new WisdomData
        {
            Id = 1, 
            Wisdom = "Что разум человека может постигнуть и во что он может поверить, того он способен достичь"
        });

        configuration.HasData(new WisdomData
        {
            Id = 2,
            Wisdom = "Стремитесь не к успеху, а к ценностям, которые он дает​"
        });
        
        configuration.HasData(new WisdomData
        {
            Id = 3,
            Wisdom = "Своим успехом я обязана тому, что никогда не оправдывалась и не принимала оправданий от других."
        });

        configuration.HasData(new WisdomData
        {
            Id = 4,
            Wisdom =
                "За свою карьеру я пропустил более 9000 бросков, проиграл почти 300 игр. 26 раз мне доверяли сделать финальный победный бросок, и я промахивался. Я терпел поражения снова, и снова, и снова. И именно поэтому я добился успеха."
        });

        configuration.HasData(new WisdomData
        {
            Id = 5,
            Wisdom = "Сложнее всего начать действовать, все остальное зависит только от упорства."
        });

        configuration.HasData(new WisdomData
        {
            Id = 6,
            Wisdom = "Надо любить жизнь больше, чем смысл жизни."
        });

        configuration.HasData(new WisdomData
        {
            Id = 7,
            Wisdom = "Жизнь - это то, что с тобой происходит, пока ты строишь планы."
        });

        configuration.HasData(new WisdomData
        {
            Id = 8,
            Wisdom = "Логика может привести Вас от пункта А к пункту Б, а воображение — куда угодно."
        });

        configuration.HasData(new WisdomData
        {
            Id = 9,
            Wisdom =
                "Через 20 лет вы будете больше разочарованы теми вещами, которые вы не делали, чем теми, которые вы сделали. Так отчальте от тихой пристани. Почувствуйте попутный ветер в вашем парусе. Двигайтесь вперед, действуйте, открывайте!"
        });
        
        configuration.HasData(new WisdomData
        {
            Id = 10,
            Wisdom = "Начинать всегда стоит с того, что сеет сомнения."
        });
    }
}