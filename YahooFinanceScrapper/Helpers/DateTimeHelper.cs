using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YahooFinanceScrapper.Helpers;

public static class DateTimeHelper
{
    public static long ToUnixTimeMiliseconds(DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }
}
