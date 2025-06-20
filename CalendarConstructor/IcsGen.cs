﻿using SalatTimeExtractor;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalendarConstructor;

public class Calender
{
    private const string AppName = "SalatCal";
    public List<Event> Events { get; set; } = new();

    public string ToString()
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
    public Event(string uid, (DateTime DateTime, Location Location) dtstart, (DateTime DateTime, Location Location) dtend, Prayer desc)
    {
        UID = uid;
        DTSTART = (dtstart.DateTime, dtstart.Location);
        DTEND = (dtend.DateTime.AddMinutes(7), dtend.Location);
        DESCRIPTION = desc;
    }

    public string UID;
    public string SUMMARY { get; set; } = "VEVENT";
    public (DateTime DateTime, Location Location) DTSTART { get; set; }
    public (DateTime DateTime, Location Location) DTEND { get; set; }
    public Prayer DESCRIPTION { get; set; }
    public Status STATUS { get; set; } = Status.CONFIRMED; 

    public override string ToString()
    {
        return
        "\n" +
        $"{nameof(UID)}:blahblah@example.com\n" +
        $"{nameof(SUMMARY)}:{SUMMARY}\n" +
        $"{nameof(DTSTART)};TZID={DTSTART.Location.Country}/{DTSTART.Location.City}:{DTSTART.DateTime.ToString("yyyyMMdd'T'HHmmss")}\n" +
        $"{nameof(DTEND)};TZID={DTEND.Location.Country}/{DTEND.Location.City}:{DTEND.DateTime.AddMinutes(7).ToString("yyyyMMdd'T'HHmmss")}\n" +
        $"{nameof(DESCRIPTION)}:{DESCRIPTION} Prayer Time\n" +
        $"{nameof(STATUS)}:{STATUS}" +
        "\n"; 

    }
}


public enum Status
{ 
    CANCLLED,
    TENTATIVE,
    CONFIRMED
}


