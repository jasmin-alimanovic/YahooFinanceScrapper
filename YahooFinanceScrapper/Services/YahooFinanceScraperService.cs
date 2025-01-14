using YahooFinanceScrapper.Builders;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Services;

public class YahooFinanceScraperService(ITickerRepository tickerRepository) : IYahooFinanceScraperService
{
    public async Task<Ticker> ScrapeAndSaveTickerData(string ticker, DateTime startDate)
    {
        try
        {
            var ticketBuilder = TickerBuilder.Builder()
                .WithTickerName(ticker)
                .WithDate(startDate)
                .WithCompanyName()
                .WithMarketCap()
                .WithCity()
                .WithState()
                .WithPreviousClosePrice()
                .WithNumberOfEmployees()
                .WithYearFounded()
                .WithOpenPrice()
                .Build();

            return await tickerRepository.CreateAsync(ticketBuilder);

        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<List<Ticker>> GetAllTickers(string[] tickerSymbols, DateTime date)
    {
        List<Ticker> tickers = new();

        foreach (var item in tickerSymbols)
        {
            var ticker = await tickerRepository.GetBySymbolAndDate(item, date);

            // If ticker is already in database don't scrape again just return it
            if (ticker is not null)
            {
                tickers.Add(ticker);
                continue;
            }

            tickers.Add(await ScrapeAndSaveTickerData(item, date));
        }

        return tickers;
    }
}
