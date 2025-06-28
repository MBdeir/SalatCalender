using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalatTimeExtractor;

namespace SalatCalender;

public class Salat
{
    private readonly ILogger<Salat> _logger;

    public Salat(ILogger<Salat> logger)
    {
        _logger = logger;
    }

    [Function("Test")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"salat.ics")] HttpRequest req)
    {
        const City Sydney = City.Sydney;  

        var salatTime = await Scrapper.Init(Sydney);

        Calender todaysEvents = new Calender();
        foreach (var prayer in salatTime.Prayers)
        {
            var builtEvent = new Event(prayer.PrayerTime, prayer.PrayerName, Sydney);
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