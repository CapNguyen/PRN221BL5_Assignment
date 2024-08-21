using Assignment3.Hub;
using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace Assignment3.Pages.Attendees
{
    public class ListModel : PageModel
    {
        private readonly IAttendee service;
        private readonly IHubContext<MyHub> hub;
        [BindProperty]
        public List<Attendee> Attendees { get; set; }

        public ListModel(IAttendee service, IHubContext<MyHub> hub)
        {
            this.service = service;
            this.hub = hub;
        }

        public async Task<IActionResult> OnGet()
        {
            Attendees = await service.All();
            return Page();
        } 
    }
}
