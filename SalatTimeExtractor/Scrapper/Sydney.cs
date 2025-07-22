using HtmlAgilityPack;

namespace SalatTimeExtractor;

public class Sydney : IScrapper
{
    public string URL { get; } = "https://www.aljaafaria.com.au";

    public List<Prayer> Prayers { get; } = new();

    public Location Location { get; } = Location.SetLocation(City.Sydney);

    private HtmlNodeCollection _rows;

    public async Task Run() 
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(URL);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        _rows = doc.DocumentNode.SelectNodes("//div[contains(@class,'ptime-row')]");

        if (_rows == null)
        {
            Console.WriteLine("No prayer time rows found.");
            return;
        }

        foreach (PrayerEnum prayer in Enum.GetValues(typeof(PrayerEnum)))
        {
            var prayerTime = Today(prayer.ToString());

            if (!string.IsNullOrEmpty(prayerTime))
            {
                Prayers.Add(
                    new Prayer
                    {
                        PrayerName = prayer,
                        PrayerTime = HelperMethods.Parse(prayerTime, Location)
                    });
            }
        }

        var Maghrib = Prayers.Where(x => x.PrayerName == PrayerEnum.Maghrib).FirstOrDefault();

        if (Location.NowLocal > Maghrib.PrayerTime)
        {
            foreach (PrayerEnum prayer in Enum.GetValues(typeof(PrayerEnum)))
            {
                var prayerTime = Tommorow(prayer.ToString());

                if (!string.IsNullOrEmpty(prayerTime))
                {
                    Prayers.Add(
                        new Prayer
                        {
                            PrayerName = prayer,
                            PrayerTime = HelperMethods.Parse(prayerTime, Location).AddDays(1)
                        });
                }
            }
        }
    }

    public string Today(string prayerName)
    {
        foreach (var row in _rows)
        {
            var titleNode = row.SelectSingleNode(".//span[contains(@class,'ptime-title')]");
            if (titleNode != null && titleNode.InnerText.Trim().Equals(prayerName.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var timeNode = row.SelectSingleNode($".//div[contains(@class,'col-xs-4')][2]/span");
                if (timeNode != null)
                {
                    return HtmlEntity.DeEntitize(timeNode.InnerText.Split('<')[0].Trim());
                }
            }
        }
        return string.Empty;
    }

    public string Tommorow(string prayerName)
    {
        foreach (var row in _rows)
        {
            var titleNode = row.SelectSingleNode(".//span[contains(@class,'ptime-title')]");
            if (titleNode != null && titleNode.InnerText.Trim().Equals(prayerName.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var timeNode = row.SelectSingleNode($".//div[contains(@class,'col-xs-4')][3]/span");
                if (timeNode != null)
                {
                    return HtmlEntity.DeEntitize(timeNode.InnerText.Split('<')[0].Trim());
                }
            }
        }
        return string.Empty;
    }
}