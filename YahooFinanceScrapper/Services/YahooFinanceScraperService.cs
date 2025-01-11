using HtmlAgilityPack;
using YahooFinanceScrapper.Builders;
using YahooFinanceScrapper.Constants;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Repositories;

namespace YahooFinanceScrapper.Services;

public class YahooFinanceScraperService(ITickerRepository tickerRepository) : IYahooFinanceScraperService
{
    public async Task ScrapeAndSaveTickerData(string ticker, DateTime startDate)
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
                .Build();

            await tickerRepository.CreateAsync(ticketBuilder);

        }
        catch (Exception e)
        {
            throw;
        }
    }
}
