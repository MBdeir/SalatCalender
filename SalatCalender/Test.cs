using CalendarConstructor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalatTimeExtractor;

namespace SalatCalender;

public class Test
{
    private readonly ILogger<Test> _logger;

    public Test(ILogger<Test> logger)
    {
        _logger = logger;
    }

    [Function("Test")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        var salatTime = await Scrapper.Init();

        Calender todaysEvents = new Calender();
        foreach (var prayer in salatTime.Prayers)
        {
            var builtEvent = new Event(
                prayer.PrayerName.ToString(),
                (prayer.PrayerTime, Location.SetLocation(City.Sydney)),
                (prayer.PrayerTime, Location.SetLocation(City.Sydney)),
                prayer.PrayerName
                );
            todaysEvents.Events.Add(builtEvent);
        }

        var ics = todaysEvents.ToString();

        var filePath = Path.Combine("C:\\Users\\moeyb\\Code\\Mohammad\\SalatCalender\\SalatCalender", "calendar.ics");
        await File.WriteAllTextAsync(filePath, ics);

        return new OkResult();
    }
}