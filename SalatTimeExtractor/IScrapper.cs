namespace SalatTimeExtractor;

public interface IScrapper
{
    Location Location { get; }

    List<Prayer> Prayers { get; }
    
    IWebPage WebPage { get; }
}
