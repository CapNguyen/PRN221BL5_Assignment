using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Services.Implement;

public class AttendeeService : IAttendee
{
    private readonly PRN221_BL5_Assignment3Context db;

    public AttendeeService(PRN221_BL5_Assignment3Context db)
    {
        this.db = db;
    }

    public async Task<List<Attendee>> All()
    {
        return await db.Attendees.Include(a => a.User).Include(a => a.Event).ToListAsync();
    }

    public async Task<bool> Register(Attendee a)
    {
        var ev = await db.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.EventId == a.EventId);
        var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == a.UserId);
        if (ev == null || user == null)
        {
            return false;
        }
        a.Event = ev;
        a.User = user;
        db.Attendees.Add(a);
        await db.SaveChangesAsync();
        return true;
    }
}