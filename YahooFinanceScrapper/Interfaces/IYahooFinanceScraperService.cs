using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Interfaces;

public interface IYahooFinanceScraperService
{
    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and https://finance.yahoo.com/quote/{ticker}/history/ websites and returns ticker object
    /// </summary>
    /// <param name="ticker"></param>
    /// <param name="startDate"></param>
    /// <returns></returns>
    public Task<Ticker> ScrapeAndSaveTickerData(string ticker, DateTime startDate);

    /// <summary>
    /// Returns list of tickers
    /// </summary>
    /// <param name="tickerSymbols"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<List<Ticker>> GetAllTickers(string[] tickerSymbols, DateTime date);
}