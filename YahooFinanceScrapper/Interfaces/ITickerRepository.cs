using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Interfaces;

public interface ITickerRepository : IBaseRepository<Ticker>
{
    public Task<Ticker?> GetBySymbolAndDate(string tickerSymbol, DateTime date);
}
