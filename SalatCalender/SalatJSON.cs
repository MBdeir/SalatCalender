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

            // var prayers = await Scrapper.Init(Sydney);
            var location = Location.SetLocation(Sydney);

            var IHIC = new IHIC();

            await IHIC.Run();
            
            var result = new SalatJSONResult
            {
                TimeZone = location.TimeZone.Id,
                Fajr = HelperMethods.ToSimple(IHIC.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Fajr).PrayerTime),
                Duhur = HelperMethods.ToSimple(IHIC.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Duhur).PrayerTime),
                Maghrib = HelperMethods.ToSimple(IHIC.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Maghrib).PrayerTime),
            };

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to build Salat JSON response");
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public class SalatJSONResult
    {
        public string TimeZone { get; set; } = string.Empty;

        public string Fajr { get; set; }
        public string Duhur { get; set; }
        public string Asr { get; set; }
        public string Maghrib { get; set; }
        public string Isha { get; set; }
    }
}
