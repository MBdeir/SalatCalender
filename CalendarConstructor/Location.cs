namespace CalendarConstructor;

public class Location
{
    public Country Country { get; }
    public City City { get; }

    public Location(Country country, City city)
    {
        if (!LocationMap.TryGetValue(city, out var expectedCountry) || expectedCountry != country)
        {
            throw new ArgumentException($"City {city} does not belong to country {country}");
        }

        Country = country;
        City = city;
    }

    private static readonly Dictionary<City, Country> LocationMap = new()
    {
        { City.Sydney, Country.Australia },
    };
}

public enum Country
{
    Australia
}

public enum City
{
    Sydney,
    Brisbane,
    Melbourne,
    Perth
}