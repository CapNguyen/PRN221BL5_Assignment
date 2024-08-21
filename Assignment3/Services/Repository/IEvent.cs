using Assignment3.Models;

namespace Assignment3.Services.Repository;

public interface IEvent
{
    Task<Event> GetEvent(int? id);
    Task<List<Event>> ListEvents();
    Task<bool> AddEvent(Event ev);
    Task<bool> UpdateEvent(Event ev);
    Task<bool> DeleteEvent(int eventId);
}