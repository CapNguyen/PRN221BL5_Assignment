using Assignment3.Hub;
using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace Assignment3.Pages.Events;

public class Add : PageModel
{
    private IEvent eventServices;
    private ICategory categoryService;
    private readonly IHubContext<MyHub> hub;
    [BindProperty]
    public Event Event { get; set; }

    [BindProperty]
    public List<EventCategory> Categories { get; set; }
    public Add(IEvent eventServices, ICategory categoryService, IHubContext<MyHub> hub)
    {
        this.eventServices = eventServices;
        this.categoryService = categoryService;
        this.hub = hub;
    }

    public async Task<IActionResult> OnGet()
    {
        return await LoadData();
    }

    private async Task<IActionResult> LoadData()
    {
        Categories = await categoryService.ListCategories();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var check = await eventServices.AddEvent(Event);
        if (check)
        {
            await hub.Clients.All.SendAsync("ReloadItem");
            return RedirectToPage("/Events/List");
        }
        else
        {
            return await LoadData();
        }
    }
}