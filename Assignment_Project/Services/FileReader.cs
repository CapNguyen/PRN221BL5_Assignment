using Assignment_Project.Models;

namespace Assignment_Project.Services
{
    public class FileReader
    {
        private TimeSlotService timeSlotService = new TimeSlotService();
        public List<Lesson> ReadCSV(IFormFile file)
        {
            List<Lesson> lessons = new List<Lesson>();
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        List<Lesson> data = ProcessLine(line);
                        if (data != null)
                        {
                            lessons.AddRange(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in reading file: {ex.Message}");
            }
            return lessons;
        }

        private List<Lesson> ProcessLine(string line)
        {
            List<Lesson> lessons = new List<Lesson>();
            try
            {
                string[] fields = line.Split(',');
                foreach (string field in fields)
                {
                    if (field == null || "".Equals(field))
                    {
                        lessons = null;
                    }
                }
                if (fields.Length == 6)
                {
                    Dictionary<int, TimeSlot> weekday_slot = ProcessTimeSlot(fields[4]);
                    foreach (var kvp in weekday_slot)
                    {
                        Lesson l = new Lesson
                        {
                            Class = fields[0],
                            Subject = fields[1],
                            Teacher = fields[2],
                            Room = fields[3],
                            TimeslotId = kvp.Value.Id,
                            Weekday = kvp.Key,
                            Week = int.Parse(fields[5]),
                        };
                        lessons.Add(l);
                    }

                }
                else
                {
                    Console.WriteLine($"Error: {line}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing line: {ex.Message}");
            }

            return lessons;
        }
        private Dictionary<int, TimeSlot> ProcessTimeSlot(string data)
        {
            Dictionary<int, TimeSlot> result = new Dictionary<int, TimeSlot>();

            if (data.Length != 3)
            {
                return null;
            }
            if (data[0] == 'A')
            {
                for (int i = 1; i < data.Length; i++)
                {
                    if (int.TryParse(data[i].ToString(), out int value) && value != 0)
                    {

                        TimeSlot t = timeSlotService.GetBySlot(i);
                        if (t != null)
                        {
                            result.Add(value, t);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Err: {data[i]} cannot parse or = 0 ");
                    }

                }
            }
            if (data[0] == 'P')
            {
                for (int i = 3; i < data.Length + 2; i++)
                {
                    if (int.TryParse(data[i].ToString(), out int value) && value != 0)
                    {

                        TimeSlot t = timeSlotService.GetBySlot(i);
                        if (t != null)
                        {
                            result.Add(value, t);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Err: {data[i]} cannot parse or = 0 ");
                    }

                }
            }
            return result;
        }
    }
}
