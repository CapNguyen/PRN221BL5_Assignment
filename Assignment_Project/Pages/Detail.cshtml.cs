using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment_Project.Models;
using Assignment_Project.Services;

namespace Assignment_Project.Pages
{
    public class DetailModel : PageModel
    {
        private LessonService lessonService = new LessonService();
        private TimeSlotService timeSlotService = new TimeSlotService();
        public List<TimeSlot> TimeSlots { get; set; }
        [BindProperty]
        public Lesson Lesson { get; set; }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Class { get; set; }
        [BindProperty]
        public string Subject { get; set; }
        [BindProperty]
        public string Teacher { get; set; }
        [BindProperty]
        public string Room { get; set; }
        [BindProperty]
        public int TimeSlotId { get; set; }
        [BindProperty]
        public int Week { get; set; }
        [BindProperty]
        public int WeekDay { get; set; }

        public void OnGet()
        {
            int? id = null;
            if (Request.RouteValues["lessonId"] != null)
            {
                id = int.Parse(Request.RouteValues["lessonId"].ToString());
                Lesson = lessonService.GetById(id);
            }
            TimeSlots = timeSlotService.GetAll();
        }
        public void OnPost()
        {
            lessonService.Update(Lesson);
            TimeSlots = timeSlotService.GetAll();
        }
        public IActionResult OnPostDelete(int id)
        {
            lessonService.Delete(id);
            return RedirectToPage("/Schedule");
        }
    }
}
