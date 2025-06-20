using SalatTimeExtractor;
using System.Text;

namespace CalendarConstructor;

public class Calender
{
    private const string AppName = "SalatCal";
    public static List<Event> Events { get; set; } = new();

    public static string ToString()
    {
        StringBuilder sb = new StringBuilder(
            "BEGIN:VCALENDAR\n" +
            "VERSION:1.0\n" +
            $"PRODID:-//{AppName}//Prayer Times//EN\n" +
            "CALSCALE:GREGORIAN\n"
            );

        foreach (var _event in Events)
        {
            sb.Append(
                "BEGIN:VEVENT\n" +
                _event.ToString()+
                "END:VEVENT\n"
                ) ;
        }

        sb.Append("END:VCALENDAR");
        return sb.ToString();
    }
}
public class Event
{
    public string UID { get; set; }
    public string SUMMARY { get; set; }
    public (DateTime DateTime, Location Location) DTSTART { get; set; }
    public (DateTime DateTime, Location Location) DTEND { get; set; }
    public Prayer DESCRIPTION { get; set; }
    public Status STATUS { get; set; }

    public override string ToString()
    {
        return
        $"{nameof(UID)}:blahblah@example.com\n" +
        $"{nameof(SUMMARY)}:VEVENT\n" +
        $"{nameof(DTSTART)};TZID={DTSTART.Location.Country}/{DTSTART.Location.City}:{DTSTART.DateTime.ToString("yyyyMMdd'T'HHmmss")}\n" +
        $"{nameof(DTEND)};TZID={DTEND.Location.Country}/{DTEND.Location.City}:{DTSTART.DateTime.AddMinutes(7).ToString("yyyyMMdd'T'HHmmss")}\n" +
        $"{nameof(DESCRIPTION)}:{DESCRIPTION} Prayer Time" +
        $"{nameof(STATUS)}:{STATUS}";
    }
}


public enum Status
{ 
    CANCLLED,
    TENTATIVE,
    CONFIRMED
}


