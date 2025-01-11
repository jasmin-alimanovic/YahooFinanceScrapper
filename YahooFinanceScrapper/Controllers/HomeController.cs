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

        public IActionResult Index()
        {
            _yahooFinanceScrapperService.ScrapeAndSaveTickerData("UNB", DateTime.Now);
            return View();
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
