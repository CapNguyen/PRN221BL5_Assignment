using Assignment3.Models;
using Assignment3.Services.Repository;
using Assignment3.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Assignment3.Pages.Events
{
    public class ListModel : PageModel
    {
        private IEvent eventServices;
        private ICategory categoryService;
        private IUser userService;
        public List<int> PagingSize { get; set; }
        public int TotalPage { get; set; }
        public string UserRole { get; set; }

        [BindProperty]
        public int EventId { get; set; }
        [BindProperty]
        public List<Event> Events { get; set; }
        [BindProperty]
        public List<EventCategory> Categories { get; set; }
        [BindProperty]
        public string Title_Location_Description { get; set; }
        [BindProperty]
        public string CategoryId { get; set; }
        [BindProperty]
        public DateTime? From { get; set; }
        [BindProperty]
        public DateTime? To { get; set; }
        [BindProperty]
        public int? PageIndex { get; set; }
        [BindProperty]
        public int? PageSize { get; set; }

        public ListModel(IEvent eventServices, ICategory categoryService, IUser userService)
        {
            this.eventServices = eventServices;
            this.categoryService = categoryService;
            this.userService = userService;
            PagingSize = new List<int>() { 5, 10, 15 };
        }

        public async Task<IActionResult> OnGet()
        {
            return await LoadData();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            int eventId = EventId;
            var check = await eventServices.DeleteEvent(eventId);
            TempData["DeleteStatus"] = check ? "Delete Successful" : "Delete Failed";
            return await LoadData();
        }


        public async Task<IActionResult> OnPostLoad(string title_location_description, string categoryId, DateTime? from, DateTime? to, int? pageIndex, int? pageSize)
        {
            return await LoadData(title_location_description, categoryId, from, to, pageIndex, pageSize);
        }
        private async Task<IActionResult> LoadData(string title_location_description = null, string categoryId = null, DateTime? from = null, DateTime? to = null, int? pageIndex = null, int? pageSize = null)
        {
            int userId = HttpContext.Session.GetSession<int>("User");
            var user = await userService.GetUser(userId);
            if (user == null)
            {
                return RedirectToPage("/Authentication/Login");
            }
            UserRole = user.Role;
            var events = await eventServices.ListEvents();
            Categories = await categoryService.ListCategories();

            int totalItem = events.Count;
            pageIndex = pageIndex == null ? 1 : pageIndex;
            pageSize = pageSize == null ? PagingSize[0] : pageSize;
            int offset = (int)((pageIndex - 1) * pageSize);
            int pageCount = (int)(totalItem / pageSize);
            TotalPage = totalItem % pageSize == 0 ? pageCount : pageCount + 1;

            Events = events.Where(e =>
                                     (string.IsNullOrEmpty(title_location_description)
                                     || (e.Title.Contains(title_location_description)
                                     || e.Location!.Contains(title_location_description)
                                     || e.Description!.Contains(title_location_description)))
                                  && (string.IsNullOrEmpty(categoryId) || e.CategoryId == Int32.Parse(categoryId))
                                  && (!from.HasValue || !to.HasValue || (e.StartTime >= from && e.EndTime <= to)))
                .Skip(offset)
                .Take((int)pageSize)
                .ToList();

            return Page();
        }
    }
}
