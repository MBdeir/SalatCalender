namespace SalatTimeExtractor;

public static partial class Scrapper
{

    public static async Task<SalatDTO> Init(City city)
    {
        //return await Sydney.Run();
        return city switch
        {
            City.Sydney => await Sydney.Run(),
            _ => new SalatDTO()
        };
    }
}
