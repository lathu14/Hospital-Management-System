using System;
using System.Data;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class DoctorScheduleRepository
    {
        public static string NormalizeTimeSlot(string slot)
        {
            if (string.IsNullOrEmpty(slot)) return "";
            string lower = slot.ToLower().Replace(" ", "").Replace("–", "-");
            if (lower.Contains("09.00am") || lower.Contains("09.00a.m")) return "09.00 a.m - 12.00p.m";
            if (lower.Contains("12.00pm") || lower.Contains("12.00p.m")) return "12.00 p.m - 03.00p.m";
            if (lower.Contains("03.00pm") || lower.Contains("03.00p.m")) return "03.00 p.m - 06.00p.m";
            if (lower.Contains("06.00pm") || lower.Contains("06.00p.m")) return "06.00 p.m - 09.00p.m";
            return slot; // Fallback
        }

        public DataTable GetScheduleByDoctorAndDate(string doctorId, DateTime date)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ds.schedule_id AS 'Schedule ID', d.full_name AS 'Doctor Name', 
                                            ds.date AS 'Date', ds.time_slot AS 'Time Slot', ds.status AS 'Status'
                                     FROM doctor_schedules ds
                                     JOIN doctors d ON ds.doctor_id = d.doctor_id
                                     WHERE ds.doctor_id = @doctor_id AND ds.date = @date";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctor_id", doctorId);
                        cmd.Parameters.AddWithValue("@date", date.Date);
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error fetching schedule for doctor: {doctorId} on date: {date:yyyy-MM-dd}");
                throw;
            }
        }

        public bool SlotExists(string doctorId, DateTime date, string slot)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string normalized = NormalizeTimeSlot(slot);
                    string query = "SELECT COUNT(*) FROM doctor_schedules WHERE doctor_id = @doctor_id AND date = @date AND time_slot = @slot";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctor_id", doctorId);
                        cmd.Parameters.AddWithValue("@date", date.Date);
                        cmd.Parameters.AddWithValue("@slot", normalized);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error checking slot existence for doctor: {doctorId} on date: {date:yyyy-MM-dd}");
                throw;
            }
        }

        public void SaveSchedule(DoctorSchedule schedule)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string normalized = NormalizeTimeSlot(schedule.TimeSlot);
                    string query = @"INSERT INTO doctor_schedules (doctor_id, date, time_slot, status) 
                                     VALUES (@doctor_id, @date, @slot, @status)
                                     ON DUPLICATE KEY UPDATE status = @status";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctor_id", schedule.DoctorId);
                        cmd.Parameters.AddWithValue("@date", schedule.Date.Date);
                        cmd.Parameters.AddWithValue("@slot", normalized);
                        cmd.Parameters.AddWithValue("@status", schedule.Status);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error saving schedule slot for doctor: {schedule.DoctorId}");
                throw;
            }
        }
    }
}
