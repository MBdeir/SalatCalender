using SalatTimeExtractor;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalendarConstructor;

public class Calender
{
    private const string AppName = "SalatCal";
    public List<Event> Events { get; set; } = new();

    public string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("METHOD:PUBLISH");
        sb.AppendLine($"PRODID:-//{AppName}//Prayer Times//EN");
        sb.AppendLine("CALSCALE:GREGORIAN");

        foreach (var _event in Events)
        {
            sb.AppendLine(_event.ToString());
        }

        sb.Append("END:VCALENDAR");
        return sb.ToString();
    }
}
public class Event
{
    public Event(DateTime dtstart, Prayer desc, City location)
    {
        DTSTART = dtstart;
        DESCRIPTION = desc;
        Location = Location.SetLocation(location);
    }

    public string UID;
    public string SUMMARY { get; set; }
    public DateTime DTSTART { get; set; }
    public DateTime DTEND { get; set; }
    public Prayer DESCRIPTION { get; set; }

    public Location Location { get; set; }
    public Status STATUS { get; set; } = Status.CONFIRMED; 

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VEVENT");
        sb.AppendLine($"UID:{Guid.NewGuid()}");
        sb.AppendLine($"SUMMARY:{DESCRIPTION} Prayer Time");
        sb.AppendLine($"DSTART;TZID={Location.Country}/{Location.City}:{DTSTART.ToString("yyyyMMdd'T'HHmmss")}");
        //sb.AppendLine($"DTEND;TZID={Location.Country}/{Location.City}:{DTEND.ToString("yyyyMMdd'T'HHmmss")}");
        //sb.AppendLine("DESCRIPTION:{DESCRIPTION} Prayer Time");
        sb.AppendLine($"STATUS:{STATUS}");
        sb.AppendLine("END:VEVENT");
        return sb.ToString();
    }
}


public enum Status
{ 
    CANCLLED,
    TENTATIVE,
    CONFIRMED
}


