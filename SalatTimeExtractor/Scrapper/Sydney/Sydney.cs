namespace SalatTimeExtractor;

public class Sydney : IScrapper
{
    public Sydney() : this(SydneyWeb.SMCA)
    {
    }

    public Sydney(SydneyWeb preferredWebpage)
    {
        WebPage = preferredWebpage switch
        {
            SydneyWeb.IHIC => new IHIC(),
            SydneyWeb.SMCA => new SMCA(),
            _ => throw new ArgumentOutOfRangeException(nameof(preferredWebpage), preferredWebpage, null)
        };
    }

    public enum SydneyWeb
    {
        IHIC,
        SMCA
    }

    public IWebPage WebPage { get; }

    public List<Prayer> Prayers => WebPage.Prayers;

    public Location Location { get; } = Location.SetLocation(City.Sydney);
}
