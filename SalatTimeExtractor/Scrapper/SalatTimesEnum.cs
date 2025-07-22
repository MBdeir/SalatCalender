namespace SalatTimeExtractor;

public enum PrayerEnum
{
    Fajr,
    Duhur,
    Asr,
    Maghrib,
    Isha
}

public class SalatDTO
{
    public List<Prayer> Prayers { get; set; } = new();
}

public class Prayer
{
    public PrayerEnum PrayerName { get; set; }

    public string PrayerTime { get; set; }
}