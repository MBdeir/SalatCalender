namespace SalatTimeExtractor;

public static partial class Scrapper
{
    public static async Task<List<Prayer>> Init(City city)
    {
        IScrapper scrapper =  city switch
        {
            City.Sydney => new Sydney(),
            _           => throw new Exception($"No implementation for {city} yet"),
        };

        await scrapper.WebPage.Run();
        return scrapper.Prayers;
    }
}
