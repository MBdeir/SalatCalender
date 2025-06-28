using System.Text;

namespace SalatTimeExtractor;

public class Calender
{
    private const string AppName = "SalatCal";
    public List<Event> Events { get; set; } = new();

    public Location Location { get; set; }

    public Calender(SalatDTO dto, City city) 
    {
        foreach (var prayer in dto.Prayers)
        {
            Events.Add(new Event(prayer.PrayerTime, prayer.PrayerName));
        }
        
        Location = Location.SetLocation(city);
    }

    public string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("METHOD:PUBLISH");
        sb.AppendLine($"PRODID:-//{AppName}//Prayer Times//EN");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine();

        foreach (var _event in Events)
        {
            sb.AppendLine(_event.ToString(Location));
        }

        sb.Append("END:VCALENDAR");
        return sb.ToString();
    }

    public class Event
    {
        public Event(string PrayerTime, Prayer PrayerName)
        {
            DTSTART = PrayerTime;
            DESCRIPTION = PrayerName;
        }

        public string UID { get; } = Guid.NewGuid().ToString();
        public string SUMMARY { get; set; }
        public string DTSTART { get; set; }
        //public DateTime DTEND { get; set; }
        public Prayer DESCRIPTION { get; set; }
        public Status STATUS { get; } = Status.CONFIRMED;

        public string ToString(Location Location)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{UID}");
            sb.AppendLine($"SUMMARY:{DESCRIPTION} Prayer Time");
            sb.AppendLine($"DTSTART;TZID={Location.Country}/{Location.City}:{DTSTART}");
            //sb.AppendLine($"DTEND;TZID={Location.Country}/{Location.City}:{DTEND.ToString("yyyyMMdd'T'HHmmss")}");
            //sb.AppendLine("DESCRIPTION:{DESCRIPTION} Prayer Time");
            sb.AppendLine($"STATUS:{STATUS}");
            sb.AppendLine("END:VEVENT");
            return sb.ToString();
        }
    }
}

public enum Status
{ 
    CANCLLED,
    TENTATIVE,
    CONFIRMED
}


