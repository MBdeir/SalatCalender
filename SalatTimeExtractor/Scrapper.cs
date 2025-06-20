using HtmlAgilityPack;

namespace SalatTimeExtractor;

public static class Scrapper
{
    private const string url = "https://www.aljaafaria.com.au";
   
    public static async Task Init()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var rows = doc.DocumentNode.SelectNodes("//div[contains(@class,'ptime-row')]");

        if (rows == null)
        {
            Console.WriteLine("No prayer time rows found.");
            return;
        }

        string GetTime(string prayerName)
        {
            foreach (var row in rows)
            {
                var titleNode = row.SelectSingleNode(".//span[contains(@class,'ptime-title')]");
                if (titleNode != null && titleNode.InnerText.Trim().Equals(prayerName, StringComparison.OrdinalIgnoreCase))
                {
                    var timeNode = row.SelectSingleNode(".//div[contains(@class,'col-xs-4')][2]/span");
                    if (timeNode != null)
                    {
                        return HtmlEntity.DeEntitize(timeNode.InnerText.Split('<')[0].Trim());
                    }
                }
            }
            return string.Empty;
        }

        var prayersToday = new SalatDTO
        {
            Fajr = HelperMethods.String2DateTime(GetTime(nameof(SalatEnum.Fajr))),
            Duhur = HelperMethods.String2DateTime(GetTime(nameof(SalatEnum.Duhur))),
            Asr = HelperMethods.String2DateTime(GetTime(nameof(SalatEnum.Asr))),
            Maghrib = HelperMethods.String2DateTime(GetTime(nameof(SalatEnum.Maghrib))),
            Isha = HelperMethods.String2DateTime(GetTime(nameof(SalatEnum.Isha))),
        };

        Console.WriteLine($"Fajr: {prayersToday.Fajr}");
        Console.WriteLine($"Duhur: {prayersToday.Duhur}");
        Console.WriteLine($"Asr: {prayersToday.Asr}");
        Console.WriteLine($"Maghrib: {prayersToday.Maghrib}");
        Console.WriteLine($"Isha: {prayersToday.Isha}");
    }
}
