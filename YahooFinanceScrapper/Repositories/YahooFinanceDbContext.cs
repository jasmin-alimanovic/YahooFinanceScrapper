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

        modelBuilder.Entity<TickerSymbol>().HasData(
            new TickerSymbol { Symbol = "NVDA", Name = "NVIDIA Corporation" },
            new TickerSymbol { Symbol = "TSLA", Name = "Tesla, Inc." },
            new TickerSymbol { Symbol = "WBA", Name = "Walgreens Boots Alliance, Inc." },
            new TickerSymbol { Symbol = "CEG", Name = "Constellation Energy Corporatio" },
            new TickerSymbol { Symbol = "AAPL", Name = "Apple Inc." },
            new TickerSymbol { Symbol = "SHOP", Name = "Shopify Inc." },
            new TickerSymbol { Symbol = "QQQ", Name = "Invesco QQQ Trust, Series 1" },
            new TickerSymbol { Symbol = "PLTR", Name = "Palantir Technologies Inc." },
            new TickerSymbol { Symbol = "AMZN", Name = "Amazon.com, Inc." },
            new TickerSymbol { Symbol = "META", Name = "Meta Platforms, Inc." },
            new TickerSymbol { Symbol = "CVS", Name = "CVS Health Corporation" },
            new TickerSymbol { Symbol = "MSFT", Name = "Microsoft Corporation" },
            new TickerSymbol { Symbol = "INTC", Name = "Intel Corporation" },
            new TickerSymbol { Symbol = "GCTK", Name = "GlucoTrack, Inc." },
            new TickerSymbol { Symbol = "AVGO", Name = "Broadcom Inc." }
            );
    }
}
