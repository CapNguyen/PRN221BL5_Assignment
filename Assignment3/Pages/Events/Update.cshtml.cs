using Assignment3.Hub;
using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace Assignment3.Pages.Events;

public class Update : PageModel
{
    private IEvent eventServices;
    private ICategory categoryService;
    private readonly IHubContext<MyHub> hub;

    [BindProperty]
    public Event Event { get; set; }

    [BindProperty]
    public List<EventCategory> Categories { get; set; }

    public Update(IEvent eventServices, ICategory categoryService)
    {
        this.eventServices = eventServices;
        this.categoryService = categoryService;
    }

    public async Task<IActionResult> OnGet(int? eventId)
    {
        return await LoadData(eventId);
    }

    private async Task<IActionResult> LoadData(int? eventId)
    {
        Categories = await categoryService.ListCategories();
        Event = await eventServices.GetEvent(eventId);
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        var check = await eventServices.UpdateEvent(Event);
        if (check)
        {
            await hub.Clients.All.SendAsync("ReloadItem");
            return RedirectToPage("/Events/List");
        }
        else
        {
            return Page();
        }
    }
}