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
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get" )] HttpRequest req)
    {
        await Scrapper.Init();

        return new OkResult();
    }
}