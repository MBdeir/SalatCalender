
using SalatTimeExtractor;


var salatInfo = await Scrapper.Init(City.Sydney);

//salatInfo.ForEach(x => Console.WriteLine(x.PrayerName + " " + x.PrayerTime));

Console.WriteLine(HelperMethods.ToString(DateTimeOffset.Now));


