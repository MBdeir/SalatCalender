using HtmlAgilityPack;

namespace SalatTimeExtractor;

public static partial class Scrapper
{
    public class Sydney 
    {
        private const string URL = "https://www.aljaafaria.com.au";
        //possible url? https://shiaa.com.au/salat/times/2025/nsw/{city}

        private const City city = City.Sydney;
        public static async Task<SalatDTO> Run()
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(URL);

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
                string PrayerTime = GetTime(prayer);
                if (!string.IsNullOrEmpty(PrayerTime))
                {
                    var prayerToAdd = new Prayers
                    {
                        PrayerName = prayer,
                        PrayerTime = HelperMethods.ToString(PrayerTime, Location.SetLocation(city))
                    };

                    PrayersToReturn.Prayers.Add(prayerToAdd);
                }
            }
            return PrayersToReturn;
        }
    }
}
