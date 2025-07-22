using System.Globalization;
using System.Text;

namespace SalatTimeExtractor;

public static class HelperMethods
{
    public static DateTime String2DateTime(string time) => DateTime.Parse(time);

    //Expects input as H:mm tt. (Example: 5:15 PM)
    public static string ToString(string date, Location Location)
    {

        var lexer = date.Split(" ");

        var isPM = lexer.Last() == "PM";

        var time = lexer.First().Split(":");
        string minute = time.Last();

        string hour = time.First();
        if (hour.Length == 1)
        {
            hour = AppendZero(hour);
        }
        if (isPM && hour != "12")
        {
            int hourInt = int.Parse(hour) + 12;
            hour = hourInt.ToString();
        }

        if (minute.Length == 1)
        {
            minute = AppendZero(minute);
        }

        TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById($"{Location.Country}/{Location.City}");
        DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);

        string year = localDateTime.Year.ToString();
        string month = localDateTime.Month.ToString();
        string day = localDateTime.Day.ToString();

        if (month.Length == 1)
        {
            month = AppendZero(month);
        }
        if (day.Length == 1)
        {
            day = AppendZero(day);
        }

        return new StringBuilder(year + month + day + "T" + hour + minute + "00").ToString();

        //20250629T171100
    }

    static string AppendZero(string s) => "0" + s;

    public static DateTime FindLocalTime(City city)
    {
        var _location = Location.SetLocation(city);

        TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById($"{_location.Country}/{_location.City}");
        DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);
        return localDateTime;
    }

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
