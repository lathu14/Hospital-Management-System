using System;

namespace Hospital_Management_System.Models
{
    public class Patient
    {
        public string PatientId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhotoPath { get; set; }
    }
}
