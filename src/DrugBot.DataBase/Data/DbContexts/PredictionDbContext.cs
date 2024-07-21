using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class PredictionDbContext(DbContextOptions<PredictionDbContext> options) : DbContext(options)
{
    public DbSet<PredictionData> Predictions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildEmojis(modelBuilder);
    }

    private void BuildEmojis(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<PredictionData>();

        configuration.HasData(new PredictionData
        {
            Id = 1,
            Prediction = "Перед вами прямая дорога к заветной цели. Получится все, что вы задумали."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 2,
            Prediction = "Нужные люди или счастливое и удачное стечение обстоятельств помогут вам добиться желаемого."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 3, Prediction = "Препятствия, возникающие одно за другим, могут помешать выполнению ваших планов."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 4,
            Prediction =
                "Реализация целей зависит от ваших усилий. Если у вас хватит терпения следовать тому, что вы наметили, — успех возможен."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 5,
            Prediction = "Займитесь накоплением знаний, в данный момент это нужно вам больше всего."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 6,
            Prediction =
                "Шаг за шагом вы приближаетесь к намеченной цели. «Тише едешь — дальше будешь» — в данном случае для вас."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 7, Prediction = "Временные трудности и испытания. Сохраните достоинство и не теряйте из виду цель."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 8,
            Prediction =
                "Обстоятельства сложатся удачно, добавьте смекалки или силы, чтобы убрать противостояние вашим планам."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 9,
            Prediction =
                "Имейте терпение и добьетесь всего, что пожелаете. В данном случае поспешные действия неуместны.Можете рассчитывать только на плоды своих усилий. Помощь со стороны может оказаться «медвежьей услугой»."
        });
        
        configuration.HasData(new PredictionData
        {
            Id = 10,
            Prediction =
                "Вы окажетесь в выигрыше. Это будет сюрпризом, так как может получиться не в то время, в какое вы предполагаете."
        });
    }
}