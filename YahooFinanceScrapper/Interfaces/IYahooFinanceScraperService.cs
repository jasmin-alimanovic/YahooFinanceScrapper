namespace YahooFinanceScrapper.Interfaces;

public interface IYahooFinanceScraperService
{
    public Task ScrapeAndSaveTickerData(string ticker, DateTime startDate);
}