using System;

namespace Hospital_Management_System.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
