using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment_Project.Models;
using Assignment_Project.Services;

namespace Assignment_Project.Pages
{
    public class ScheduleModel : PageModel
    {
        private LessonService lessonService = new LessonService();
        private TimeSlotService timeSlotService = new TimeSlotService();
        private FileReader fileReader = new FileReader();
        public List<int>? Weeks { get; set; }
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
        public List<string> Rooms { get; set; }

        [BindProperty]
        public int Week { get; set; }
        [BindProperty]
        public KeyValuePair<int, string> WeekDay { get; set; }
        public void OnGet()
        {
            LoadData();
        }
        public void OnPost(IFormFile csvFile)
        {

            if (csvFile != null || csvFile.Length != 0)
            {
                List<Lesson> data = fileReader.ReadCSV(csvFile);
                foreach (Lesson lesson in data)
                {
                    lessonService.Add(lesson);
                }
            }
            LoadData();
        }
        public void OnPostFilter(int? week, int? weekday)
        {
            LoadData(week, weekday);
        }
        private void LoadData(int? week = null, int? weekday = null)
        {
            week = (week == null) ? 1 : week;
            weekday = (weekday == null) ? 2 : weekday;
            Lessons = lessonService.GetByWeekAndWeekDay(week, weekday);
            Rooms = lessonService.GetAllRooms();
            Weeks = lessonService.GetAllWeeks();
            TimeSlots = timeSlotService.GetAll();
        }
    }
}
