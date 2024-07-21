using Microsoft.EntityFrameworkCore;

namespace DrugBot.DataBase.Data.DbContexts;

public class ChineseDbContext(DbContextOptions<ChineseDbContext> options) : DbContext(options)
{
    public DbSet<ChineseData> ChineseSymbols { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildSymbols(modelBuilder);
    }

    private void BuildSymbols(ModelBuilder modelBuilder)
    {
        var configuration = modelBuilder.Entity<ChineseData>();

        configuration.HasData(new ChineseData{ Id = 1, ChineseSymbol = '安'});
        configuration.HasData(new ChineseData{ Id = 2, ChineseSymbol = '吧'});
        configuration.HasData(new ChineseData{ Id = 3, ChineseSymbol = '八'});
        configuration.HasData(new ChineseData{ Id = 4, ChineseSymbol = '爸'});
        configuration.HasData(new ChineseData{ Id = 5, ChineseSymbol = '百'});
        configuration.HasData(new ChineseData{ Id = 6, ChineseSymbol = '北'});
        configuration.HasData(new ChineseData{ Id = 7, ChineseSymbol = '不'});
        configuration.HasData(new ChineseData{ Id = 8, ChineseSymbol = '大'});
        configuration.HasData(new ChineseData{ Id = 9, ChineseSymbol = '岛'});
        configuration.HasData(new ChineseData{ Id = 10, ChineseSymbol = '的'});
    }
}