using System.Text;

namespace SalatTimeExtractor;

public static class HelperMethods
{
    public static DateTime String2DateTime(string time) => DateTime.Parse(time);

    //Expects input as H:mm tt. (Example: 5:15 PM)
    public static string ToString(string date, Location Location)
    {
        StringBuilder sb = new();

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

        Console.WriteLine(localDateTime);

        Console.WriteLine(localDateTime.Year);
        Console.WriteLine(localDateTime.Month);
        Console.WriteLine(localDateTime.Day);

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

        sb.Append(year + month + day + "T" + hour + minute + "00");

        Console.WriteLine(sb.ToString());
        return sb.ToString();
        //20250629T171100
    }

    static string AppendZero(string s) => "0" + s;
}
