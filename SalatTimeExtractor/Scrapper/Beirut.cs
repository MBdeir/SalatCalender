using HtmlAgilityPack;

namespace SalatTimeExtractor;

public class Beirut : IScrapper
{
    public string URL { get; set; } = "https://www.urdupoint.com/islam/shia/beirut-prayer-timings.html";
    public Location Location { get; } = Location.SetLocation(City.Beirut);

    private IEnumerable<HtmlNode> rows;

    public List<Prayer> Prayers { get; } = new();

    public DateTime LocalDateNow { get; set; }

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

        var dto = new SalatDTO();
        dto.Prayers.Add
            (
                new Prayer
                {
                    PrayerName = PrayerEnum.Fajr,
                    PrayerTime = HelperMethods.ToString(Scrape("Fajar"), Location)
                }
            );
    }

    public string Scrape(string prayerName) => Scrape(rows, prayerName);

    public string Scrape(IEnumerable<HtmlNode> rows, string prayerName) 
    {
        foreach (var row in rows)
        {
            var titleNode = row.SelectSingleNode("./th//span");
            if (titleNode != null
             && string.Equals(titleNode.InnerText.Trim(), prayerName, StringComparison.OrdinalIgnoreCase))
            {
                var timeNode = row.SelectSingleNode("./td[contains(@class,'arial') and contains(@class,'ltr')]");
                if (timeNode != null)
                    return HtmlEntity.DeEntitize(timeNode.InnerText.Trim());
            }
        }
        return string.Empty;
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
