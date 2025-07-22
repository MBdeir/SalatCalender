namespace SalatTimeExtractor;

public enum PrayerEnum
{
    Fajr,
    Duhur,
    Asr,
    Maghrib,
    Isha
}

public class Prayer
{
    public PrayerEnum PrayerName { get; set; }

    public DateTimeOffset PrayerTime { get; set; }
}