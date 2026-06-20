using System;
using System.Data;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class DoctorScheduleService
    {
        private readonly DoctorScheduleRepository _scheduleRepository = new DoctorScheduleRepository();

        public DataTable GetSchedule(string doctorId, DateTime date)
        {
            if (string.IsNullOrEmpty(doctorId)) return new DataTable();
            return _scheduleRepository.GetScheduleByDoctorAndDate(doctorId, date);
        }

        public (bool ok, string msg) AddSlot(DoctorSchedule schedule)
        {
            if (string.IsNullOrWhiteSpace(schedule.DoctorId) ||
                string.IsNullOrWhiteSpace(schedule.TimeSlot))
            {
                return (false, "Doctor ID and Time Slot are required.");
            }

            try
            {
                if (_scheduleRepository.SlotExists(schedule.DoctorId, schedule.Date, schedule.TimeSlot))
                {
                    return (false, "This schedule slot is already configured for this doctor.");
                }

                _scheduleRepository.SaveSchedule(schedule);
                return (true, "Schedule Slot Saved Successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error adding schedule slot for doctor: {schedule.DoctorId}");
                return (false, "An error occurred while saving the schedule.");
            }
        }
    }
}
