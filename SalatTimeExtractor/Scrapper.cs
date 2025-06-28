using CalendarConstructor;
using HtmlAgilityPack;

namespace SalatTimeExtractor;

public static partial class Scrapper
{

    public static async Task<SalatDTO> Init(City city)
    {
        return city switch
        {
            City.Sydney => await Sydney(),
            _ => new SalatDTO() 
        };
    }
}
