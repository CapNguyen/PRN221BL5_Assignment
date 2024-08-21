using Assignment_Project.Models;

namespace Assignment_Project.Services
{
    public class TimeSlotService
    {
        private PRN221_FapProjectContext context = new PRN221_FapProjectContext();
        public List<TimeSlot> GetAll()
        {
            return context.TimeSlots.ToList();
        }
        public TimeSlot GetBySlot(int slot)
        {
            return context.TimeSlots.FirstOrDefault(x => x.Slot == slot);
        }
        public void Add(TimeSlot timeslot)
        {
            if (checkConstraint(timeslot))
            {
                using (var context = new PRN221_FapProjectContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.TimeSlots.Add(timeslot);
                            context.SaveChanges();
                            transaction.Commit();
                            Console.WriteLine("Add Successfully");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            transaction.Rollback();
                        }
                    }
                }
            }
        }

        private bool checkConstraint(TimeSlot timeslot)
        {
            List<TimeSlot> all = GetAll();
            bool canAdd = false;
            foreach (TimeSlot ts in all)
            {
                if (timeslot.Slot != ts.Slot && !timeslot.Time.Equals(timeslot.Time)) {
                    canAdd = true;
                }
            }
            return canAdd;
        }
    }
}
