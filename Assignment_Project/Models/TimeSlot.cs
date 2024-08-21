using System;
using System.Collections.Generic;

namespace Assignment_Project.Models
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string Time { get; set; } = null!;
        public int Slot { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
