using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;
using YahooFinanceScrapper.Constants;
using YahooFinanceScrapper.Helpers;
using YahooFinanceScrapper.Models;

namespace YahooFinanceScrapper.Builders;

public interface TickerNameStep
{
    DateStep WithTickerName(string ticker);
}

public interface DateStep
{
    SummarySteps WithDate(DateTime date);
}

public interface SummarySteps
{
    TickerBuilder WithCompanyName();
    TickerBuilder WithMarketCap();
    TickerBuilder WithNumberOfEmployees();
    TickerBuilder WithYearFounded();
    TickerBuilder WithCity();
    TickerBuilder WithState();
    TickerBuilder WithPreviousClosePrice();
    TickerBuilder WithOpenPrice();
}

public class TickerBuilder : TickerNameStep, DateStep, SummarySteps
{
    private readonly Ticker _data = new();
    private HtmlDocument _tickerHistoryHtml;
    private HtmlDocument _tickerSummaryHtml;
    const string Unknown = "N/A";

    public static TickerNameStep Builder()
    {
        return new TickerBuilder();
    }

    public DateStep WithTickerName(string ticker)
    {
        _data.TickerName = ticker;
        return this;
    }

    public SummarySteps WithDate(DateTime date)
    {
        _data.Date = date;

        // Initialize the ticker summary html
        var url = $"{Constant.BASE_STOCK_URL}/{_data.TickerName}";
        var web = new HtmlWeb();
        _tickerSummaryHtml = web.Load(url);

        // Initialize the ticker history html
        var startDateMiliseconds = DateTimeHelper.ToUnixTimeMiliseconds(date);
        var endDateMiliseconds = DateTimeHelper.ToUnixTimeMiliseconds(date.AddDays(1));
        url = $"{Constant.BASE_STOCK_URL}/{_data.TickerName}/history/?period1={startDateMiliseconds}&period2={endDateMiliseconds}";
        _tickerHistoryHtml = web.Load(url);

        return this;
    }
    public TickerBuilder WithCompanyName()
    {
        var companyName = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//h1[@class='yf-xxbei9']");
        _data.CompanyName = companyName.InnerText;
        return this;
    }

    public TickerBuilder WithMarketCap()
    {
        var marketCap = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//fin-streamer[@data-field='marketCap']");
        _data.MarketCap = marketCap.InnerText;
        
        return this;
    }

    public TickerBuilder WithNumberOfEmployees()
    {
        var employees = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//p[@class='yf-1swrzsh']");
        _data.CompanyName = employees.InnerText;
        return this;
    }

    public TickerBuilder WithYearFounded()
    {
        throw new NotImplementedException();
    }

    public TickerBuilder WithCity()
    {
        var cityAndState = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[contains(@class,'description') and contains(@class, 'yf-1swrzsh')]");
        var cityAndStateTxt = GetCityAndState(cityAndState?.FirstChild?.InnerText);
        _data.HeadquartersCity = cityAndStateTxt != Unknown ? cityAndStateTxt.Split(',').ElementAt(0) : Unknown;
        return this;
    }

    public TickerBuilder WithState()
    {
        var cityAndState = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//div[contains(@class,'description') and contains(@class, 'yf-1swrzsh')]");
        var cityAndStateTxt = GetCityAndState(cityAndState?.FirstChild?.InnerText);
        _data.HeadquartersState = cityAndStateTxt != Unknown ? cityAndStateTxt.Split(',').ElementAt(1) : Unknown;
        return this;
    }

    public TickerBuilder WithPreviousClosePrice()
    {
        var prevousClose = _tickerSummaryHtml.DocumentNode.SelectSingleNode("//fin-streamer[@data-field='regularMarketPreviousClose']");
        _data.PreviousClosePrice = Convert.ToDecimal(prevousClose?.InnerText);

        return this;

    }

    public TickerBuilder WithOpenPrice()
    {
        throw new NotImplementedException();
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
            string result = input.Substring(startIndex, input.IndexOf('.')).Trim();
            result = result.TrimEnd('.');

            return result;
        }

        return Unknown;
    }
}
