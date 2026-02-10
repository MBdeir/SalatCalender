namespace SalatTimeExtractor;

public static partial class Scrapper
{
    public static async Task<List<Prayer>> Init(City city)
    {
        IScrapper scrapper =  city switch
        {
            City.Sydney => new Sydney(),
            City.Beirut => new Beirut(),
            _           => throw new Exception($"No implementation for {city} yet"),
        };

        await scrapper.Run();
        return scrapper.Prayers;
    }
}
