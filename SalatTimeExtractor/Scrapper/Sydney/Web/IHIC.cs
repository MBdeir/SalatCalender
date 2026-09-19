namespace SalatTimeExtractor;

using HtmlAgilityPack;
using Microsoft.Playwright;

public class IHIC : IWebPage
{
    public string URL { get; } = "https://www.ihic.org.au";

    public List<Prayer> Prayers { get; } = new();
    
    private static readonly Dictionary<string, string> PrayerSpanClasses = new()
    {
        { nameof(PrayerEnum.Fajr), "fajr" },
        { nameof(PrayerEnum.Duhur), "dhuhr" },
        { nameof(PrayerEnum.Maghrib), "maghrib" },
    };
    
    private async Task<string> FetchHtml()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        var response = await page.GotoAsync(URL, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

        if (response == null || !response.Ok)
        {
            throw new Exception($"Scrape failed. Status={response?.Status}.");
        }

        return await page.ContentAsync();
    }
    
    public string Today(string prayerName)
    {
        var lines = _csv.Split('\n');
        if (lines.Length < 2)
        {
            return string.Empty;
        }

        var headers = lines[0].Split(',');
        var values = lines[1].Split(',');

        var index = Array.IndexOf(headers, prayerName);
        return index >= 0 && index < values.Length ? values[index] : string.Empty;
        
    }
    
    private string _csv = string.Empty;
    
    public async Task Run()
    {
        var html = await FetchHtml();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var prayerTimesNode = doc.DocumentNode.SelectSingleNode("//div[@id='prayer_times']");

        if (prayerTimesNode == null)
        {
            var title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText ?? "(no title)";
            var snippet = html.Length > 500 ? html[..500] : html;
            throw new Exception($"No #prayer_times div found. Page title: '{title}'. HTML start: {snippet}");
        }

        var header = string.Join(',', PrayerSpanClasses.Keys);
        var values = PrayerSpanClasses.Values.Select(spanClass =>
        {
            var timeNode = prayerTimesNode.SelectSingleNode($".//span[contains(@class,'{spanClass}')]");
            return timeNode != null
                ? HtmlEntity.DeEntitize(timeNode.InnerText).Replace(" ", string.Empty).Trim()
                : string.Empty;
        });

        _csv = $"{header}\n{string.Join(',', values)}";

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
    }
    
    public string Tomorrow(string prayerName) => string.Empty; 
    
    public Location Location { get; } = Location.SetLocation(City.Sydney);

}