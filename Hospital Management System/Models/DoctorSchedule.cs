using System;

namespace Hospital_Management_System.Models
{
    public class DoctorSchedule
    {
        public int ScheduleId { get; set; }
        public string DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
    }
}
