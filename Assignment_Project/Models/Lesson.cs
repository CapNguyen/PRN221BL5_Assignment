using System;
using System.Collections.Generic;

namespace Assignment_Project.Models
{
    public partial class Lesson
    {
        public int Id { get; set; }
        public string Class { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Teacher { get; set; } = null!;
        public string Room { get; set; } = null!;
        public int TimeslotId { get; set; }
        public int Weekday { get; set; }
        public int Week { get; set; }

        public virtual TimeSlot Timeslot { get; set; } = null!;
    }
}
