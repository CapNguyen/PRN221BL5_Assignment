using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Assignment_Project.Models;
using System.Transactions;

namespace Assignment_Project.Services
{
    public class LessonService
    {
        private PRN221_FapProjectContext context = new PRN221_FapProjectContext();

        public List<Lesson> GetAll()
        {
            return context.Lessons.ToList();
        }
        public List<int> GetAllWeeks()
        {
            return context.Lessons.Select(l => l.Week).Distinct().ToList();
        }

        public List<string> GetAllRooms()
        {
            return context.Lessons.Select(l => l.Room).Distinct().ToList();
        }

        public Lesson GetById(int? id)
        {
            return context.Lessons.Include(l => l.Timeslot).First(l => l.Id == id);
        }

        public List<Lesson> GetByWeekAndWeekDay(int? week, int? weekDay)
        {
            return context.Lessons.Where(l => (weekDay == null || l.Weekday == weekDay) && (week == null || l.Week == week)).ToList();
        }
        public void Add(Lesson lesson)
        {
            if (CheckConstraint(lesson))
            {
                using (var context = new PRN221_FapProjectContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Lessons.Add(lesson);
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
            else
            {
                Console.WriteLine("Something ain't right :D");
            }
        }
        public void Delete(int id)
        {
            Lesson l = context.Lessons.FirstOrDefault(l => l.Id == id);
            if (l != null)
            {
                context.Lessons.Remove(l);
                context.SaveChanges();
                Console.WriteLine("Del Succesfully");
            }
            else
            {
                Console.WriteLine("Null Lesson");
            }
        }
        public void Update(Lesson lesson)
        {
            Lesson l = context.Lessons.FirstOrDefault(l => l.Id == lesson.Id);
            if (l != null)
            {
                try
                {
                    context.Attach(l);
                    context.Entry(l).State = EntityState.Modified;
                    context.SaveChanges();
                    Console.WriteLine("Edit Succesfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
            else
            {
                Console.WriteLine("Null Lesson");
            }
        }
        private bool CheckConstraint(Lesson lesson)
        {
            bool canBeAddOrEdit = false;
            List<Lesson> all = GetAll();
            foreach (Lesson l in all)
            {
                if (lesson.Week == l.Week && lesson.Weekday == l.Weekday)
                {
                    if (lesson.Room.Equals(l.Room))
                    {
                        canBeAddOrEdit = (lesson.Timeslot != l.Timeslot);
                    }
                    else if (lesson.Timeslot == l.Timeslot)
                    {
                        canBeAddOrEdit = (lesson.Teacher != l.Teacher && lesson.Class != l.Class);
                    }
                }
                else
                {
                    canBeAddOrEdit = true;
                }
            }
            return canBeAddOrEdit;
        }

    }
}
