using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalatTimeExtractor;

public class Calender
{
    private const string AppName = "SalatCal";
    public List<Event> Events { get; set; } = new();
    public Location Location { get; set; }

    public Calender(List<Prayer> Prayers, City city)
    {
        foreach (var prayer in Prayers)
        {
            Events.Add(new Event(prayer.PrayerName, prayer.PrayerTime));
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
        public Event(PrayerEnum PrayerName, DateTimeOffset PrayerTime)
        {
            DTSTART = HelperMethods.ToString(PrayerTime);
            Prayer = PrayerName;
        }

        public string UID { get; } = Guid.NewGuid().ToString();
        public string SUMMARY { get; set; }
        public string DTSTART { get; set; }

        //public DateTime DTEND { get; set; }
        public PrayerEnum Prayer { get; set; }
        public Status STATUS { get; } = Status.CONFIRMED;

        public string ToString(Location Location)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{UID}");
            sb.AppendLine($"SUMMARY:{Prayer} Prayer");
            sb.AppendLine($"DTSTART;TZID={Location.Country}/{Location.City}:{DTSTART}");
            //sb.AppendLine($"DTEND;TZID={Location.Country}/{Location.City}:{DTEND.ToString("yyyyMMdd'T'HHmmss")}");
            //sb.AppendLine("DESCRIPTION:{DESCRIPTION} Prayer Time");
            sb.AppendLine($"STATUS:{STATUS}");
            sb.Append(new Alarm().ToString(Prayer));
            sb.AppendLine("END:VEVENT");
            return sb.ToString();
        }
        public class Alarm
        {
            private const int TimeLeftForPrayer = 15;
            public Alarm() 
            {
            }
            
            public string ToString(PrayerEnum Prayer)
            {
                var sb = new StringBuilder();
                sb.AppendLine("BEGIN:VALARM");
                sb.AppendLine("ACTION:DISPLAY");
                sb.AppendLine("TRIGGER:-PT15M");
                sb.AppendLine($"DESCRIPTION:{Prayer} Prayer is in {TimeLeftForPrayer} minutes");
                sb.AppendLine("END:VALARM");
                return sb.ToString();
            }
        }
    }
}

public enum Status
{
    CANCLLED,
    TENTATIVE,
    CONFIRMED
}


