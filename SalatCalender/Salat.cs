using CalendarConstructor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalatTimeExtractor;
using System.Net;

namespace SalatCalender;

public class Salat
{
    private readonly ILogger<Salat> _logger;

    public Salat(ILogger<Salat> logger)
    {
        _logger = logger;
    }

    [Function("Test")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "head", "options", Route = "salat.ics")] HttpRequest req)
    {
        
        var salatTime = await Scrapper.Init();

        Calender todaysEvents = new Calender();
        foreach (var prayer in salatTime.Prayers)
        {
            var builtEvent = new Event(
                Guid.NewGuid().ToString(),
                (prayer.PrayerTime, Location.SetLocation(City.Sydney)),
                (prayer.PrayerTime, Location.SetLocation(City.Sydney)),
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
    }
}