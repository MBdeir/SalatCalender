using CalendarConstructor;
using HtmlAgilityPack;

namespace SalatTimeExtractor;

public static partial class Scrapper
{
    private const string url = "https://www.aljaafaria.com.au";

    public static async Task<SalatDTO> Init(City city)
    {
        return city switch
        {
            City.Sydney => await Sydney(),
            _ => new SalatDTO() 
        };
    }
}
