namespace SalatCalender;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalatTimeExtractor;

public class SalatJSON(ILogger<SalatJSON> logger)
{
    [Function("SalatJSON")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "salat.json")] HttpRequest req)
    {
        try
        {
            const City Sydney = City.Sydney;

            var prayers = await Scrapper.Init(Sydney);
            var location = Location.SetLocation(Sydney);

            // Past today's Maghrib the list also contains tomorrow's prayers; this endpoint is today only.
            var today = location.NowLocal.Date;
            var todays = prayers.Where(p => p.PrayerTime.Date == today).ToList();

            var result = new SalatJSONResult
            {
                TimeZone = location.TimeZone.Id,
                Fajr = Format(todays, PrayerEnum.Fajr),
                Duhur = Format(todays, PrayerEnum.Duhur),
                Maghrib = Format(todays, PrayerEnum.Maghrib),
            };

            if (result.Fajr is null || result.Duhur is null || result.Maghrib is null)
            {
                throw new Exception("Scrape produced incomplete prayer times.");
            }

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to build Salat JSON response");
            return new BadRequestObjectResult(ex.Message);
        }
    }

    private static string? Format(List<Prayer> prayers, PrayerEnum prayer)
    {
        var match = prayers.FirstOrDefault(p => p.PrayerName == prayer);
        return match is null ? null : HelperMethods.ToSimple(match.PrayerTime);
    }

    public class SalatJSONResult
    {
        public string TimeZone { get; set; } = string.Empty;

        public string? Fajr { get; set; }
        public string? Duhur { get; set; }
        public string? Maghrib { get; set; }
    }
}
