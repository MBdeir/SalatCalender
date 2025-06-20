using HtmlAgilityPack;

namespace SalatTimeExtractor;

public static class Scrapper
{
    private const string url = "https://www.aljaafaria.com.au";

    public static async Task<SalatDTO> Init()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var rows = doc.DocumentNode.SelectNodes("//div[contains(@class,'ptime-row')]");

        if (rows == null)
        {
            Console.WriteLine("No prayer time rows found.");
            return new SalatDTO { };
        }

        string GetTime(Prayer prayerName)
        {
            foreach (var row in rows)
            {
                var titleNode = row.SelectSingleNode(".//span[contains(@class,'ptime-title')]");
                if (titleNode != null && titleNode.InnerText.Trim().Equals(prayerName.ToString(), StringComparison.OrdinalIgnoreCase))
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

        var PrayersToReturn = new SalatDTO();

        foreach (Prayer prayer in Enum.GetValues(typeof(Prayer)))
        {
            if (string.IsNullOrEmpty(GetTime(prayer)))
            {
                var prayerToAdd = new Prayers
                {
                    PrayerName = prayer
                };

                PrayersToReturn.Prayers.Add(prayerToAdd);
            }
        }


        return PrayersToReturn;
    }
}
