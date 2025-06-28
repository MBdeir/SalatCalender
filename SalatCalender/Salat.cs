using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalatTimeExtractor;
using static SalatTimeExtractor.Calender;

namespace SalatCalender;

public class Salat
{
    private readonly ILogger<Salat> _logger;

    public Salat(ILogger<Salat> logger)
    {
        _logger = logger;
    }

    [Function("Salat")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "salat.ics")] HttpRequest req)
    {
        const City Sydney = City.Sydney;  

        var salatInfo = await Scrapper.Init(Sydney);

        var ics = new Calender(salatInfo, Sydney).ToString();

        return new ContentResult
        {
            Content = ics,
            ContentType = "text/calendar",
            StatusCode = 200
        };
    }
}