namespace SalatTimeExtractor;

public interface IScrapper
{
    string URL { get; }
    
    DateTime LocalDateNow { get; } //in the format of dd/mm/yyyy
    
    City city { get; }
    
    Task Run(); 

    string Today(string prayerName);

    string Tommorow(string prayerName);

    List<Prayer> Prayers { get; }
}
