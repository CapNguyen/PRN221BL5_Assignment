using Assignment3.Models;

namespace Assignment3.Services.Repository;

public interface IAttendee
{
    Task<List<Attendee>> All();
    Task<bool> Register(Attendee a);
}