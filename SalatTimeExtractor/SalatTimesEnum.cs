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
    //public Status Status { get; set; } = Status.CONFIRMED;
    public Prayer PrayerName { get; set; }

    public DateTime PrayerTime { get; set; }

}