namespace SalatTimeExtractor;

public interface IScrapper
{
    string URL { get; }
    
    Location Location { get; }
    
    Task Run(); 

    string Today(string prayerName);

    string Tommorow(string prayerName);

    List<Prayer> Prayers { get; }
}
