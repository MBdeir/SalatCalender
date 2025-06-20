namespace CalendarConstructor;

public class Calender
{
    public List<Event> Events { get; set; }
}
public class Event
{
    public string UID { get; set; }
    public string SUMMARY { get; set; }
    public (string, Location) DTSTART { get; set; }
    public (string, Location) DTEND { get; set; }
    public string DESCRIPTION { get; set; }
    public string STATUS { get; set; }

}




