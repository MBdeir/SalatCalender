using System.Text.Json;
using SalatTimeExtractor;

var outputPath = args.Length > 0 ? args[0] : "data/salat.json";

var ihic = new IHIC();
await ihic.Run();

var fajr = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Fajr)?.PrayerTime;
var duhur = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Duhur)?.PrayerTime;
var asr = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Asr)?.PrayerTime;
var maghrib = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Maghrib)?.PrayerTime;
var isha = ihic.Prayers.FirstOrDefault(p => p.PrayerName == PrayerEnum.Isha)?.PrayerTime;

if (fajr is null || duhur is null || maghrib is null)
{
    throw new Exception("Scrape produced incomplete prayer times; refusing to write output.");
}

var result = new SalatExport
{
    GeneratedAt = DateTimeOffset.UtcNow,
    TimeZone = ihic.Location.TimeZone.Id,
    Fajr = HelperMethods.ToSimple(fajr.Value),
    Duhur = HelperMethods.ToSimple(duhur.Value),
    Asr = asr is null ? null : HelperMethods.ToSimple(asr.Value),
    Maghrib = HelperMethods.ToSimple(maghrib.Value),
    Isha = isha is null ? null : HelperMethods.ToSimple(isha.Value),
};

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

    public string? Fajr { get; set; }
    public string? Duhur { get; set; }
    public string? Asr { get; set; }
    public string? Maghrib { get; set; }
    public string? Isha { get; set; }
}
