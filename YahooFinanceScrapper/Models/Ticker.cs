namespace YahooFinanceScrapper.Models
{
    public class Ticker
    {
        public int Id { get; set; }
        public string TickerName { get; set; }
        public string CompanyName { get; set; }
        public string MarketCap { get; set; }
        public string YearFounded { get; set; }
        public int NumberOfEmployees { get; set; }
        public string HeadquartersCity { get; set; }
        public string HeadquartersState { get; set; }
        public DateTime Date { get; set; }
        public DateTime ScrapedAt { get; set; }
        public decimal PreviousClosePrice { get; set; }
        public decimal OpenPrice { get; set; }
    }
}
