using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Services.Implement;

public class EventService : IEvent
{
    private PRN221_BL5_Assignment3Context db;

    public EventService(PRN221_BL5_Assignment3Context db)
    {
        this.db = db;
    }
    public async Task<Event> GetEvent(int? id)
    {
        return await db.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.EventId == id);
    }
    public async Task<List<Event>> ListEvents()
    {
        var list = await db.Events
            .Include(e => e.Category)
            .ToListAsync();
        return list;
    }

    public async Task<bool> AddEvent(Event ev)
    {
        var check = CheckEventValid(ev);
        if (check)
        {
            var cate = db.EventCategories.FirstOrDefault(c => c.CategoryId == ev.CategoryId);
            ev.Category = cate;
            db.Events.Add(ev);
            await db.SaveChangesAsync();
        }
        return check;
    }

    public async Task<bool> DeleteEvent(int eventId)
    {
        var ev = await db.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
        if (ev != null)
        {
            db.Events.Remove(ev);
            await db.SaveChangesAsync();
        }
        return ev != null;
    }


    public async Task<bool> UpdateEvent(Event ev)
    {
        var check = CheckEventValid(ev);
        if (check)
        {
            var cate = db.EventCategories.FirstOrDefault(c => c.CategoryId == ev.CategoryId);
            ev.Category = cate;
            db.Events.Update(ev);
            await db.SaveChangesAsync();
        }
        return check;
    }
    private bool CheckEventValid(Event ev)
    {
        var list = db.Events.Include(e => e.Category).Where(e => e.EventId != ev.EventId && e.Location == ev.Location).ToList();
        var isValid = list.Count == 0;
        foreach (var e in list)
        {
            List<DateTime> d1 = GetDatesBetween(e.StartTime, e.EndTime);
            List<DateTime> d2 = GetDatesBetween(ev.StartTime, ev.EndTime);
            var intersectList = d1.Intersect(d2);
            isValid = intersectList.Count() == 0;
        }
        return isValid;
    }
    private List<DateTime> GetDatesBetween(DateTime? startDate, DateTime? endDate)
    {
        List<DateTime> dates = new List<DateTime>();
        if (startDate == null || endDate == null)
        {
            return dates;
        }
        DateTime start = startDate.Value;
        DateTime end = endDate.Value;
        if (start > end)
        {
            return dates;
        }
        for (DateTime date = start; date <= end; date = date.AddDays(1))
        {
            dates.Add(date);
        }
        return dates;
    }

}