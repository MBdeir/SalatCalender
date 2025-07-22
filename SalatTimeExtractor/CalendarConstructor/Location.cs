namespace SalatTimeExtractor;

public class Location
{
    public Country Country { get; }
    public City City { get; }
    public TimeZoneInfo TimeZone { get; }

    private Location(City city, Country country, TimeZoneInfo timeZone)
    {
        City = city;
        Country = country;
        TimeZone = timeZone;
    }

    public static Location SetLocation(City city)
    {
        return city switch
        {
            City.Sydney => new Location(City.Sydney, Country.Australia, GetTimeZone("Australia/Sydney", "AUS Eastern Standard Time")),
            City.Beirut => new Location(City.Beirut, Country.Lebanon, GetTimeZone("Asia/Beirut", "Middle East Standard Time")),
            _ => throw new Exception("Country not supported yet")
        };
    }

    public DateTimeOffset NowLocal => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZone);

    private static TimeZoneInfo GetTimeZone(string iana, string windows)
    {
        try   { return TimeZoneInfo.FindSystemTimeZoneById(iana); }
        catch { return TimeZoneInfo.FindSystemTimeZoneById(windows); }
    }
}

#region Enums
public enum Country
{
    Australia,
    Lebanon
}

public enum City
{
    Sydney,
    Beirut
} 
#endregion