using System.Globalization;
using System.Text;

namespace SalatTimeExtractor;

public static class HelperMethods
{
    //Example Output
    //20250629T171100
    public static string ToString(DateTimeOffset PrayerTime) => PrayerTime.ToString("yyyyMMdd'T'HHmmss", CultureInfo.InvariantCulture);

    public static DateTimeOffset Parse(string raw, Location loc)
    {
        string[] Formats = { "h:mm tt", "hh:mm tt" };

        if (!DateTime.TryParseExact(raw.Trim(), Formats, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out var t))
            throw new FormatException($"Cannot parse time: '{raw}'");

        // Local “today” in that time zone
        var nowLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, loc.TimeZone);

        var localDateTime = nowLocal.Date.Add(t.TimeOfDay);
        var offset = loc.TimeZone.GetUtcOffset(localDateTime);
        return new DateTimeOffset(localDateTime, offset);
    }
}
