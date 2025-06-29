using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalatTimeExtractor;

public static partial class Scrapper
{
    public class Beirut
    {
        private const string URL = "https://www.urdupoint.com/islam/shia/beirut-prayer-timings.html";

        private const City city = City.Beirut;
        public static async Task<SalatDTO> Run()
        {
            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(URL);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // grab all <tr> in our prayer_table
            var rows = doc.DocumentNode
                          .SelectNodes("//table[contains(@class,'prayer_table')]//tr")
                      ?? Enumerable.Empty<HtmlNode>();

            if (!rows.Any())
            {
                Console.WriteLine("No prayer time rows found.");
                return new SalatDTO();
            }

            string GetTime(string prayerName)
            {
                foreach (var row in rows)
                {
                    var titleNode = row.SelectSingleNode("./th//span");
                    if (titleNode != null
                     && string.Equals(titleNode.InnerText.Trim(), prayerName, StringComparison.OrdinalIgnoreCase))
                    {
                        // first <td> with the time
                        var timeNode = row.SelectSingleNode("./td[contains(@class,'arial') and contains(@class,'ltr')]");
                        if (timeNode != null)
                            return HtmlEntity.DeEntitize(timeNode.InnerText.Trim());
                    }
                }
                return string.Empty;
            }


            Console.WriteLine(GetTime("Fajar"));


            var dto = new SalatDTO();
            dto.Prayers.Add
                (
                    new Prayers
                    {
                        PrayerName = Prayer.Fajr,
                        PrayerTime = HelperMethods.ToString(GetTime("Fajar"), Location.SetLocation(city))
                    }
                );




            //foreach (Prayer prayer in Enum.GetValues(typeof(Prayer)))
            //{
            //    var txt = GetTime(prayer.ToString());
            //    if (!string.IsNullOrEmpty(txt))
            //    {
            //        dto.Prayers.Add(new Prayers
            //        {
            //            PrayerName = prayer,
            //            PrayerTime = HelperMethods.ToString(txt, Location.SetLocation(city))
            //        });
            //    }
            //}

            return dto;
        }
    }
}
