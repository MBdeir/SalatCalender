using System.Globalization;

namespace SalatTimeExtractor;

public static class HelperMethods
{
    //Example Output
    //20250629T171100
    public static string ToString(DateTimeOffset PrayerTime) => PrayerTime.ToString("yyyyMMdd'T'HHmmss", CultureInfo.InvariantCulture);

    public static DateTimeOffset Parse(string raw, City city) => Parse(raw, Location.SetLocation(city));

    public static DateTimeOffset Parse(string raw, Location loc)
    {
        // Local “today” in that time zone
        var nowLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, loc.TimeZone);

        return Parse(raw, loc, nowLocal.Date);
    }

    public static DateTimeOffset Parse(string raw, Location loc, DateTime date)
    {
        string[] Formats = { "h:mm tt", "hh:mm tt", "H:mm", "HH:mm" };

        if (!DateTime.TryParseExact(raw.Trim(), Formats, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out var t))
            throw new FormatException($"Cannot parse time: '{raw}'");

        var localDateTime = date.Date.Add(t.TimeOfDay);
        var offset = loc.TimeZone.GetUtcOffset(localDateTime);
        return new DateTimeOffset(localDateTime, offset);
    }

    public static string ToSimple(DateTimeOffset time) => time.ToString("h:mm tt", CultureInfo.InvariantCulture);
}
