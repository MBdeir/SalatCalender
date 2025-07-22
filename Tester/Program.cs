
using SalatTimeExtractor;

var location = Location.SetLocation(City.Beirut);

Console.WriteLine(location.City);
Console.WriteLine(location.Country);
Console.WriteLine(location.TimeZone);
Console.WriteLine(location.NowLocal);

