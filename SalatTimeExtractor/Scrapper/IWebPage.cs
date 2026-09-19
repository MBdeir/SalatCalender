namespace SalatTimeExtractor;

public interface IWebPage
{
    string URL { get; }

    Location Location { get; }

    List<Prayer> Prayers { get; }

    Task Run();

    string Today(string prayerName);

    string Tomorrow(string prayerName);
}
