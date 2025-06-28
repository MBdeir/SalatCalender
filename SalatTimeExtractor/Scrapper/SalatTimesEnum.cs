namespace SalatTimeExtractor;

public enum Prayer
{
    Fajr,
    Duhur,
    Asr,
    Maghrib,
    Isha
}

public class SalatDTO
{
    public List<Prayers> Prayers { get; set; } = new();
}

public class Prayers
{
    public Prayer PrayerName { get; set; }

    public string PrayerTime { get; set; }
}