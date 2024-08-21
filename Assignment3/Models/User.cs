namespace Assignment3.Models;

public class User
{
    public User()
    {
        Attendees = new HashSet<Attendee>();
    }

    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; }
}