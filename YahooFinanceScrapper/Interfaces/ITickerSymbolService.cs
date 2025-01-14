using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Interfaces;

public interface ITickerSymbolService
{
    public Task<List<TickerSymbol>> GetAllAsync();
}
