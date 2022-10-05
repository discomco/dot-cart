using System.Globalization;

namespace DotCart;

public static class DateTimeUtils
{
    public const string DateFormat = "dd/MM/yyyy";
    public static TimeSpan UtcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
    public static DateTime UtcTime => DateTime.Now - UtcOffset;
    public static DateTime UtcToday => UtcTime.Date;
    public static DateTime? ParseDate(string date)
    {
        return DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture);
    }
}