//using System;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;

//namespace SalatCalender;

//public class Function1
//{
//    private readonly ILogger _logger;

//    public Function1(ILoggerFactory loggerFactory)
//    {
//        _logger = loggerFactory.CreateLogger<Function1>();
//    }

//    [Function("Function1")]
//    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
//    {
//        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
//        if (myTimer.ScheduleStatus is not null)
//        {
//            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
//        }
//    }
//}