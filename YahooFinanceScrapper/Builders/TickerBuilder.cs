using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using YahooFinanceScrapper.Constants;
using YahooFinanceScrapper.Helpers;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Builders;

public interface ITickerNameStep
{
    /// <summary>
    /// Initializes ticker symbol
    /// </summary>
    /// <param name="ticker"></param>
    /// <returns></returns>
    IDateStep WithTickerName(string ticker);
}

public interface IDateStep
{
    /// <summary>
    /// Initializes date and time and fetches html data from https://finance.yahoo.com/quote/{ticker}/ and https://finance.yahoo.com/quote/{ticker}/history/
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    ISummarySteps WithDate(DateTime date);
}

public interface ISummarySteps
{
    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and assignes company name found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithCompanyName();


    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and assignes market cap found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithMarketCap();


    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and assignes number of employees found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithNumberOfEmployees();

    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and assignes year founded found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithYearFounded();

    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and assignes headquarters city found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithCity();

    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker} and assignes headquarters state found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithState();

    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker}/history/ and assignes previous close price found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithPreviousClosePrice();

    /// <summary>
    /// Scrapes https://finance.yahoo.com/quote/{ticker}/history/ and assignes open price found on website to ticker object
    /// </summary>
    /// <returns></returns>
    TickerBuilder WithOpenPrice();
}

public class TickerBuilder : ITickerNameStep, IDateStep, ISummarySteps
{
    private readonly Ticker _data = new();
    private HtmlDocument _tickerHistoryHtml;
    private HtmlDocument _tickerSummaryHtml;
    const string Unknown = "N/A";

    public static ITickerNameStep Builder()
    {
        return new TickerBuilder();
    }

    public IDateStep WithTickerName(string ticker)
    {
        _data.TickerName = ticker;
        return this;
    }

    public ISummarySteps WithDate(DateTime date)
    {

        LoadHtmlDocuments(date);

        try
        {
            _data.Date = date;

            var dateNode = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[@slot='marketTimeNotice']");
            _data.ClosedPriceDate = ParseDateAndTime(dateNode?.InnerText?.Trim());
        }
        catch
        {
            _data.ClosedPriceDate = date;
        }
        return this;
    }

    private DateTime ParseDateAndTime(string? text)
    {
        if (text is null)
        {
            return DateTime.UtcNow;
        }

        string atCloseprefix = "At close: ";
        string asOfprefix = "As of ";

        if (text.StartsWith(atCloseprefix))
        {
            // Remove the prefix
            var dateText = text.Substring(atCloseprefix.Length).Trim();
            
            // Define format
            string format = "MMMM dd 'at' h:mm:ss tt 'EST'";
            
            return DateTime.ParseExact(dateText, format, CultureInfo.InvariantCulture);
        }

        string suffix = " EST. Market Open.";

        // Remove the prefix and suffix
        string timeText = text.Substring(asOfprefix.Length, text.Length - asOfprefix.Length - suffix.Length);

        // Define the format
        string formatTime = "hh:mm:ss tt";

        // Parse the time
        DateTime time = DateTime.ParseExact(timeText, formatTime, CultureInfo.InvariantCulture); 
        
        return new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, time.Hour, time.Minute, time.Second);

    }

    private void LoadHtmlDocuments(DateTime date)
    {
        // Initialize the ticker summary html
        var url = $"{Constant.BASE_STOCK_URL}/{HttpUtility.UrlEncode(_data.TickerName)}";
        var web = new HtmlWeb();
        _tickerSummaryHtml = web.Load(url);

        // Initialize the ticker history html
        var startDateMiliseconds = DateTimeHelper.ToUnixTimeMiliseconds(date.Date);
        var endDateMiliseconds = DateTimeHelper.ToUnixTimeMiliseconds(date.Date.AddDays(1));

        url = $"{Constant.BASE_STOCK_URL}/{_data.TickerName}/history/?period1={startDateMiliseconds}&period2={endDateMiliseconds}";
        _tickerHistoryHtml = web.Load(url);
    }

    public TickerBuilder WithCompanyName()
    {
        try
        {
            var companyName = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//h1[@class='yf-xxbei9']");
            _data.CompanyName = companyName?.InnerText;
        }
        catch
        {
            _data.CompanyName = null;
        }
        return this;
    }

    public TickerBuilder WithMarketCap()
    {
        try
        {
            var marketCap = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//fin-streamer[@data-field='marketCap']");
            _data.MarketCap = marketCap?.InnerText;
        }
        catch
        {
            _data.MarketCap = null;
        }
        return this;
    }

    public TickerBuilder WithNumberOfEmployees()
    {
        try
        {
            var employees = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[contains(@class,'right') and contains(@class, 'yf-1swrzsh')]/div[contains(@class, 'infoSection')]/p");
            _data.NumberOfEmployees = employees?.InnerText;
        }
        catch
        {
            _data.NumberOfEmployees = null;
        }
        return this;
    }

    public TickerBuilder WithYearFounded()
    {
        try
        {
            var yearFounded = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[contains(@class,'description') and contains(@class, 'yf-1swrzsh')]");
            _data.YearFounded = ExtractFoundedYear(yearFounded?.InnerText);
        }
        catch
        {
            _data.YearFounded = null;
        }
        return this;
    }

    public TickerBuilder WithCity()
    {
        try
        {
            var cityAndState = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[contains(@class,'description') and contains(@class, 'yf-1swrzsh')]");
            var cityAndStateTxt = GetCityAndState(cityAndState?.FirstChild?.InnerText);
            _data.HeadquartersCity = cityAndStateTxt != Unknown ? cityAndStateTxt?.Split(',').ElementAt(0) : Unknown;
        }
        catch
        {
            _data.HeadquartersCity = Unknown;
        }
        return this;
    }

    public TickerBuilder WithState()
    {
        try
        {
            var cityAndState = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[contains(@class,'description') and contains(@class, 'yf-1swrzsh')]");
            var cityAndStateTxt = GetCityAndState(cityAndState?.FirstChild?.InnerText);
            _data.HeadquartersState = cityAndStateTxt != Unknown ? cityAndStateTxt?.Split(',').ElementAt(1) : Unknown;
        } 
        catch
        {
            _data.HeadquartersState = Unknown;
        }
        return this;
    }

    public TickerBuilder WithPreviousClosePrice()
    {
        try
        {
            var previousClosePrice = _tickerHistoryHtml.DocumentNode.SelectSingleNode("//tbody/tr")?.ChildNodes;
            _data.PreviousClosePrice = Convert.ToDecimal(previousClosePrice?.ElementAt(4)?.InnerText);
        } 
        catch
        {
            _data.PreviousClosePrice = null;
        }
        return this;

    }

    public TickerBuilder WithOpenPrice()
    {
        try
        {
            var openPrice = _tickerHistoryHtml.DocumentNode.SelectSingleNode("//tbody/tr").ChildNodes.ElementAt(1);
            _data.OpenPrice = Convert.ToDecimal(openPrice?.InnerText);
        } 
        catch
        {
            _data.OpenPrice = null;
        }

        return this;
    }

    public Ticker Build()
    {
        return _data;
    }

    private string GetCityAndState(string? input)
    {
        if (input is null)
        {
            return Unknown;
        }
        string keyword = "headquartered in";

        int startIndex = input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);

        // Check if the keyword exists in the input
        if (startIndex != -1)
        {
            // Move the starting index to the end of the keyword
            startIndex += keyword.Length;

            // Extract the substring starting from the adjusted index
            string result = input.Substring(startIndex).Trim();
            result = result.Split(".").ElementAt(0);

            return result;
        }

        return Unknown;
    }

    public static int? ExtractFoundedYear(string? input)
    {
        if (input is null)
        {
            return null;
        }

        string pattern = @"\b(founded\s+in\s+|was\s+founded\s+in\s+|incorporated\s+in\s+)(\d{4})\b";

        // Match the pattern in the input string
        Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        // If a match is found, extract the year and return it
        if (match.Success && int.TryParse(match.Groups[2].Value, out int year))
        {
            return year;
        }

        return null;
    }
}
