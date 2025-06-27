using CalendarConstructor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalatTimeExtractor;
using System.Net;

namespace SalatCalender;

public class Test
{
    private readonly ILogger<Test> _logger;

    public Test(ILogger<Test> logger)
    {
        _logger = logger;
    }

    [Function("Test")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "salat.ics")] HttpRequest req)
    {
        
        var salatTime = await Scrapper.Init();

        Calender todaysEvents = new Calender();
        foreach (var prayer in salatTime.Prayers)
        {
            var builtEvent = new Event(
                 prayer.PrayerName.ToString(), (
                 prayer.PrayerTime, Location.SetLocation(City.Sydney)), (
                 prayer.PrayerTime, Location.SetLocation(City.Sydney)),
                 prayer.PrayerName);
            todaysEvents.Events.Add(builtEvent);
        }

        var ics = todaysEvents.ToString();

        //var filePath = Path.Combine("C:\\Users\\moeyb\\Code\\Mohammad\\SalatCalender\\SalatCalender", "calendar.ics");
        //await File.WriteAllTextAsync(filePath, ics);

        return new ContentResult
        {
            Content = ics,
            ContentType = "text/calendar",
            StatusCode = 200
        };

        //return new ContentResult
        //{
        //    Content = "BEGIN:VCALENDAR\r\nVERSION:1.0\r\nPRODID:-//SalatCal//Prayer Times//EN\r\nCALSCALE:GREGORIAN\r\nBEGIN:VEVENT\r\n\r\nUID:blahblah@example.com\r\nSUMMARY:VEVENT\r\nDTSTART;TZID=Australia/Sydney:20250621T054500\r\nDTEND;TZID=Australia/Sydney:20250621T055900\r\nDESCRIPTION:Fajr Prayer Time\r\nSTATUS:CONFIRMED\r\nEND:VEVENT\r\nBEGIN:VEVENT\r\n\r\nUID:blahblah@example.com\r\nSUMMARY:VEVENT\r\nDTSTART;TZID=Australia/Sydney:20250621T120200\r\nDTEND;TZID=Australia/Sydney:20250621T121600\r\nDESCRIPTION:Duhur Prayer Time\r\nSTATUS:CONFIRMED\r\nEND:VEVENT\r\nBEGIN:VEVENT\r\n\r\nUID:blahblah@example.com\r\nSUMMARY:VEVENT\r\nDTSTART;TZID=Australia/Sydney:20250621T170800\r\nDTEND;TZID=Australia/Sydney:20250621T172200\r\nDESCRIPTION:Maghrib Prayer Time\r\nSTATUS:CONFIRMED\r\nEND:VEVENT\r\nEND:VCALENDAR",
        //    ContentType = "text/calendar",
        //    StatusCode = 200
        //};
    }
}