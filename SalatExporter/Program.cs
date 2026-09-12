using System.Text.Json;
using SalatTimeExtractor;

var outputPath = args.Length > 0 ? args[0] : "data/salat.json";

var ihic = new IHIC();
await ihic.Run();

var result = new SalatExport
{
    GeneratedAt = DateTimeOffset.UtcNow,
    TimeZone = ihic.Location.TimeZone.Id,
    Fajr = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Fajr)?.PrayerTime,
    Duhur = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Duhur)?.PrayerTime,
    Asr = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Asr)?.PrayerTime,
    Maghrib = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Maghrib)?.PrayerTime,
    Isha = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Isha)?.PrayerTime,
};

if (result.Fajr is null || result.Duhur is null || result.Maghrib is null)
{
    throw new Exception("Scrape produced incomplete prayer times; refusing to write output.");
}

var directory = Path.GetDirectoryName(outputPath);
if (!string.IsNullOrEmpty(directory))
{
    Directory.CreateDirectory(directory);
}

var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText(outputPath, json);

Console.WriteLine($"Wrote {outputPath}");
Console.WriteLine(json);

class SalatExport
{
    public DateTimeOffset GeneratedAt { get; set; }

    public string TimeZone { get; set; } = string.Empty;

    public DateTimeOffset? Fajr { get; set; }
    public DateTimeOffset? Duhur { get; set; }
    public DateTimeOffset? Asr { get; set; }
    public DateTimeOffset? Maghrib { get; set; }
    public DateTimeOffset? Isha { get; set; }
}
