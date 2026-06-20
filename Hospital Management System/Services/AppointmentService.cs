using System;
using System.Data;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();
        private readonly PatientRepository _patientRepository = new PatientRepository();
        private readonly DoctorRepository _doctorRepository = new DoctorRepository();
        private readonly DoctorScheduleRepository _scheduleRepository = new DoctorScheduleRepository();

        public (bool ok, string msg) Book(Appointment appt)
        {
            if (string.IsNullOrWhiteSpace(appt.PatientId) ||
                string.IsNullOrWhiteSpace(appt.DoctorId) ||
                string.IsNullOrWhiteSpace(appt.TimeSlot) ||
                string.IsNullOrWhiteSpace(appt.Reason))
            {
                return (false, "Please fill in all fields (Patient ID, Doctor, Time Slot, and Reason).");
            }

            try
            {
                // 1. Verify patient exists
                if (!_patientRepository.PatientIdExists(appt.PatientId))
                {
                    return (false, $"Patient ID '{appt.PatientId}' is not registered. Please register the patient first.");
                }

                // 2. Verify doctor exists
                if (!_doctorRepository.DoctorIdExists(appt.DoctorId))
                {
                    return (false, $"Doctor ID '{appt.DoctorId}' does not exist.");
                }

                // 3. Verify schedule availability
                string normalizedSlot = DoctorScheduleRepository.NormalizeTimeSlot(appt.TimeSlot);
                if (_scheduleRepository.SlotExists(appt.DoctorId, appt.AppointmentDate, normalizedSlot))
                {
                    // Check if status is booked or blocked
                    var scheduleTable = _scheduleRepository.GetScheduleByDoctorAndDate(appt.DoctorId, appt.AppointmentDate);
                    foreach (DataRow row in scheduleTable.Rows)
                    {
                        if (row["Time Slot"].ToString() == normalizedSlot && row["Status"].ToString() != "Available")
                        {
                            return (false, "This time slot is already booked or unavailable.");
                        }
                    }
                }

                // 4. Save Appointment
                appt.Status = "Scheduled";
                _appointmentRepository.Create(appt);

                // 5. Update/Insert Doctor Schedule to Booked
                var schedule = new DoctorSchedule
                {
                    DoctorId = appt.DoctorId,
                    Date = appt.AppointmentDate,
                    TimeSlot = appt.TimeSlot,
                    Status = "Booked"
                };
                _scheduleRepository.SaveSchedule(schedule);

                return (true, "Appointment Confirmed Successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error booking appointment for patient: {appt.PatientId}");
                return (false, "An error occurred while booking the appointment.");
            }
        }

        public DataTable Search(string nameOrId)
        {
            if (string.IsNullOrWhiteSpace(nameOrId))
            {
                return _appointmentRepository.GetAll();
            }
            return _appointmentRepository.Search(nameOrId);
        }

        public DataTable GetAll()
        {
            return _appointmentRepository.GetAll();
        }
    }
}
