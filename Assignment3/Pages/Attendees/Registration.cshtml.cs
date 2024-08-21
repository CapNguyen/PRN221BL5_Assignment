using Assignment3.Hub;
using Assignment3.Models;
using Assignment3.Services.Implement;
using Assignment3.Services.Repository;
using Assignment3.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace Assignment3.Pages.Attendees;

public class Registration : PageModel
{
    private readonly IEvent eventService;
    private readonly IUser userService;
    private readonly IAttendee attendeeService;
    private readonly IHubContext<MyHub> hub; 

    [BindProperty]
    public Attendee Attendee { get; set; }

    public Event Event { get; set; }
    public User User { get; set; }

    public Registration(IEvent eventService, IUser userService, IAttendee attendeeService, IHubContext<MyHub> hub)
    {
        this.eventService = eventService;
        this.userService = userService;
        this.attendeeService = attendeeService;
        this.hub = hub;
    }

    public async Task<IActionResult> OnGet(int? eventId)
    {
        int userId = HttpContext.Session.GetSession<int>("User");
        User = await userService.GetUser(userId);
        Event = await eventService.GetEvent(eventId);

        if (User == null)
        {
            return RedirectToPage("/Authentication/Login");
        }


        Attendee = new Attendee
        {
            UserId = User.UserId,
            Name = User.FullName,
            Email = User.Email,
            EventId = Event.EventId,
            RegistrationTime = DateTime.Now
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var check = await attendeeService.Register(Attendee);
        await hub.Clients.All.SendAsync("ReloadItem");
        return RedirectToPage("/Attendees/List");
    }
}
