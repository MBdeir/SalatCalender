namespace SalatTimeExtractor;

using System.Globalization;
using System.Text.Json;

// SMCA (https://smca.net.au/prayertime/) publishes a full-year timetable as JSON:
//   times -> <city key> -> "MM-dd" -> { Fajr, Sunrise, Dhuhr, Sunset, Maghreb, Midnight }
// A copy is embedded in this assembly (Scrapper/Sydney/Static) so there is no runtime
// dependency on their site, which blocks some hosting IP ranges.
public class SMCA : IWebPage
{
    private const string ResourceName = "syd-prayer-times.json";

    private static readonly Dictionary<PrayerEnum, string> PrayerKeys = new()
    {
        { PrayerEnum.Fajr, "Fajr" },
        { PrayerEnum.Duhur, "Dhuhr" },
        { PrayerEnum.Maghrib, "Maghreb" },
    };

    private static readonly Lazy<JsonDocument> Timetable = new(LoadTimetable);

    private Dictionary<string, string> _today = new();
    private Dictionary<string, string> _tomorrow = new();

    public string URL { get; } = "https://smca.net.au/prayertime/";

    public Location Location { get; } = Location.SetLocation(City.Sydney);

    public List<Prayer> Prayers { get; } = new();

    public Task Run() => Run(Location.NowLocal);

    // Once `now` is past today's Maghrib, tomorrow's prayers are appended as well.
    public Task Run(DateTimeOffset now)
    {
        var cityKey = Location.City.ToString().ToLowerInvariant();
        if (!Timetable.Value.RootElement.GetProperty("times").TryGetProperty(cityKey, out var cityTimes))
        {
            throw new Exception($"SMCA timetable has no entry for '{cityKey}'.");
        }

        now = TimeZoneInfo.ConvertTime(now, Location.TimeZone);
        var today = now.Date;
        _today = ReadDay(cityTimes, today);
        _tomorrow = ReadDay(cityTimes, today.AddDays(1));

        Prayers.Clear();
        Prayers.AddRange(BuildPrayers(_today, today));

        var maghrib = Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Maghrib);
        if (maghrib is not null && now >= maghrib.PrayerTime)
        {
            Prayers.AddRange(BuildPrayers(_tomorrow, today.AddDays(1)));
        }

        return Task.CompletedTask;
    }

    private IEnumerable<Prayer> BuildPrayers(Dictionary<string, string> day, DateTime date)
    {
        foreach (PrayerEnum prayer in Enum.GetValues(typeof(PrayerEnum)))
        {
            var prayerTime = Lookup(day, prayer.ToString());

            if (!string.IsNullOrEmpty(prayerTime))
            {
                yield return new Prayer
                {
                    PrayerName = prayer,
                    PrayerTime = HelperMethods.Parse(prayerTime, Location, date)
                };
            }
        }
    }

    public string Today(string prayerName) => Lookup(_today, prayerName);

    public string Tomorrow(string prayerName) => Lookup(_tomorrow, prayerName);

    private static JsonDocument LoadTimetable()
    {
        using var stream = typeof(SMCA).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' not found.");

        return JsonDocument.Parse(stream);
    }

    private static string Lookup(Dictionary<string, string> day, string prayerName) =>
        Enum.TryParse<PrayerEnum>(prayerName, out var prayer)
        && PrayerKeys.TryGetValue(prayer, out var key)
        && day.TryGetValue(key, out var time)
            ? time
            : string.Empty;

    private static Dictionary<string, string> ReadDay(JsonElement cityTimes, DateTime date)
    {
        var dayKey = date.ToString("MM-dd", CultureInfo.InvariantCulture);
        if (!cityTimes.TryGetProperty(dayKey, out var day))
        {
            throw new Exception($"SMCA timetable has no entry for {dayKey}.");
        }

        return PrayerKeys.Values.ToDictionary(
            key => key,
            key => day.TryGetProperty(key, out var value) ? value.GetString()?.Trim() ?? string.Empty : string.Empty);
    }
}
