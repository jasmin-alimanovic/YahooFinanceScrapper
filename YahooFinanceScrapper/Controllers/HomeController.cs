using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YahooFinanceScrapper.Interfaces;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IYahooFinanceScraperService _yahooFinanceScrapperService;
        public HomeController(ILogger<HomeController> logger, IYahooFinanceScraperService yahooFinanceScrapperService)
        {
            _logger = logger;
            _yahooFinanceScrapperService = yahooFinanceScrapperService;
        }

        public async Task<IActionResult> Index()
        {
            var tickers = await _yahooFinanceScrapperService.GetAllTickers(["NVDA", "WBA", "CEG", "ITCI", "SSL", "RPRX", "PONY", "CPRI", "SNX"], DateTime.UtcNow.AddDays(-5));
            return Json(tickers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
