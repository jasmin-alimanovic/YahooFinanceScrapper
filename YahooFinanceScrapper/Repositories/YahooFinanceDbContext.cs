using Microsoft.EntityFrameworkCore;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Repositories;

public class YahooFinanceDbContext : DbContext
{
    public YahooFinanceDbContext(DbContextOptions<YahooFinanceDbContext> options) : base(options)
    {
    }

    public DbSet<Ticker> Tickers { get; set; }

    public DbSet<TickerSymbol> TickerSymbols { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly set Id as the primary key
        modelBuilder.Entity<Ticker>()
            .HasKey(a => a.Id);

        // Explicitly set Symbol as the primary key
        modelBuilder.Entity<TickerSymbol>()
            .HasKey(a => a.Symbol);
    }
}
