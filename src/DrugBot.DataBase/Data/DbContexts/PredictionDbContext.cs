using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class PredictionDbContext(DbContextOptions<PredictionDbContext> options) : DbContext(options)
{
    public DbSet<PredictionData> Predictions { get; set; }
}