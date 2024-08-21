namespace Assignment3.Models;

public class EventCategory
{
    public EventCategory()
    {
        Events = new HashSet<Event>();
    }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; }
}