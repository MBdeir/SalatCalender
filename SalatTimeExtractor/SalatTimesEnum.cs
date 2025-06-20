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
    public DateTime Fajr { get; set; }
    public DateTime? Duhur { get; set; }
    public DateTime? Asr { get; set; }
    public DateTime? Maghrib { get; set; }
    public DateTime? Isha { get; set; }
}
