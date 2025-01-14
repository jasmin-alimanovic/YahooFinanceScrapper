using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Services;

public class TickerSymbolService(ITickerSymbolRepository tickerSymbolRepository) : ITickerSymbolService
{
    public async Task<List<TickerSymbol>> GetAllAsync()
    {
        return await tickerSymbolRepository.GetAllAsync();
    }
}
