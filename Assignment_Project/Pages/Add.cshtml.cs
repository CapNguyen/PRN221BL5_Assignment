using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment_Project.Models;
using Assignment_Project.Services;

namespace Assignment_Project.Pages
{
    public class AddModel : PageModel
    {
        private TimeSlotService timeSlotService = new TimeSlotService();
        private LessonService lessonService = new LessonService();
        public Dictionary<int, string> WeekDays = new Dictionary<int, string>(){
                { 2, "Monday" },
                { 3, "Tuesday" },
                { 4, "Wednesday" },
                { 5, "Thursday" },
                { 6, "Friday" },
                { 7, "Saturday" },
                { 8, "Sunday" }
                };
        public List<Lesson> Lessons { get; set; }

        public List<TimeSlot> TimeSlots { get; set; }
        public void OnGet()
        {
            LoadData();
        }
        public void OnPost(Lesson lesson)
        {
            lessonService.Add(lesson);
            LoadData();
        }

        private void LoadData()
        {
            TimeSlots = timeSlotService.GetAll();
        }
    }
}
