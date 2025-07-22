using HtmlAgilityPack;

namespace SalatTimeExtractor;

public class Beirut : IScrapper
{
    public string URL { get; set; } = "https://www.urdupoint.com/islam/shia/beirut-prayer-timings.html";
    public Location Location { get; } = Location.SetLocation(City.Beirut);

    private IEnumerable<HtmlNode> rows;

    public List<Prayer> Prayers { get; } = new();

    public async Task Run()
    {
        using var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(URL);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        rows  = doc.DocumentNode.SelectNodes("//table[contains(@class,'prayer_table')]//tr")?? Enumerable.Empty<HtmlNode>();

        if (!rows.Any())
        {
            Console.WriteLine("No prayer time rows found.");
            return;
        }

        //var dto = new SalatDTO();
        //dto.Prayers.Add
        //    (
        //        new Prayer
        //        {
        //            PrayerName = PrayerEnum.Fajr,
        //            //PrayerTime = HelperMethods.ToString("Fajr", Location)
        //        }
        //    );
    }

    public string Tommorow(string prayerName) 
    {
        return string.Empty;
    }

    public string Today(string prayerName)
    {
        return string.Empty;
    }
}
