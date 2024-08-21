namespace Assignment3.Models;

public class Event
{
    public Event()
    {
        Attendees = new HashSet<Attendee>();
    }

    public int EventId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Location { get; set; }
    public int? CategoryId { get; set; }

    public virtual EventCategory? Category { get; set; }
    public virtual ICollection<Attendee> Attendees { get; set; }
}