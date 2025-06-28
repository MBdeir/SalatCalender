namespace SalatTimeExtractor;

public class Location
{
    public Country Country { get; }
    public City City { get; }

    private Location(City city, Country country)
    {
        City = city;
        Country = country;
    }

    public static Location SetLocation(City city)
    {
        Country country = city switch
        {
            City.Sydney => Country.Australia,
            City.Beirut => Country.Lebanon
        };

        return new Location(city, country);
    }
}

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