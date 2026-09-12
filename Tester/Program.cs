
using SalatTimeExtractor;


var testing = new IHIC();

await testing.Run();

testing.Prayers.ForEach(x => Console.WriteLine(x.PrayerName + " " + x.PrayerTime.ToString("h:mm tt")));

string example = HelperMethods.ToString(DateTimeOffset.Now);

Console.WriteLine(example);

foreach (var prayer in testing.Prayers)
{
    Console.WriteLine(HelperMethods.ToSimple(prayer.PrayerTime));
}





//var salatInfo = await Scrapper.Init(City.Sydney);

//salatInfo.ForEach(x => Console.WriteLine(x.PrayerName + " " + x.PrayerTime));

//Console.WriteLine(HelperMethods.ToString(DateTimeOffset.Now));


