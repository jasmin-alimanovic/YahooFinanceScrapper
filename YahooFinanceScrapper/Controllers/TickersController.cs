using Microsoft.AspNetCore.Mvc;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Controllers;
public class TickersController : Controller
{
    private readonly ILogger<TickersController> _logger;
    private readonly IYahooFinanceScraperService _yahooFinanceScrapperService;
    private readonly ITickerSymbolService _tickerSymbolService;
    public TickersController(ILogger<TickersController> logger, IYahooFinanceScraperService yahooFinanceScrapperService, ITickerSymbolService tickerSymbolService)
    {
        _logger = logger;
        _yahooFinanceScrapperService = yahooFinanceScrapperService;
        _tickerSymbolService = tickerSymbolService;
    }
    public async Task<IActionResult> Index()
    {
        var tickerSymbols = await _tickerSymbolService.GetAllAsync();
        return View(new List<Ticker>());
    }

    public async Task<IActionResult> GetTickerSymbols()
    {
        var tickerSymbols = (await _tickerSymbolService.GetAllAsync());
        return Json(tickerSymbols);
    }

    public async Task<IActionResult> GetTickers(string[] tickerSymbols, DateTime date)
    {
        var tickers = await _yahooFinanceScrapperService.GetAllTickers(tickerSymbols, date);
        return Json(tickers);
    }
}
