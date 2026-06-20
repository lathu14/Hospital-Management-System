using System;
using System.Data;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class AppointmentRepository
    {
        public void Create(Appointment appointment)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string normalizedSlot = DoctorScheduleRepository.NormalizeTimeSlot(appointment.TimeSlot);
                    string query = @"INSERT INTO appointments (patient_id, doctor_id, appointment_date, time_slot, reason, status, notes)
                                     VALUES (@patient_id, @doctor_id, @date, @slot, @reason, @status, @notes)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@patient_id", appointment.PatientId);
                        cmd.Parameters.AddWithValue("@doctor_id", appointment.DoctorId);
                        cmd.Parameters.AddWithValue("@date", appointment.AppointmentDate.Date);
                        cmd.Parameters.AddWithValue("@slot", normalizedSlot);
                        cmd.Parameters.AddWithValue("@reason", appointment.Reason);
                        cmd.Parameters.AddWithValue("@status", appointment.Status);
                        cmd.Parameters.AddWithValue("@notes", string.IsNullOrEmpty(appointment.Notes) ? DBNull.Value : (object)appointment.Notes);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error creating appointment for patient: {appointment.PatientId}");
                throw;
            }
        }

        public DataTable Search(string nameOrId)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT a.appointment_id AS 'Appointment ID', p.patient_id AS 'Patient ID', 
                                            p.full_name AS 'Patient Name', d.full_name AS 'Doctor Name', 
                                            a.appointment_date AS 'Date', a.time_slot AS 'Time Slot', 
                                            a.reason AS 'Reason', a.status AS 'Status', a.notes AS 'Notes'
                                     FROM appointments a
                                     JOIN patients p ON a.patient_id = p.patient_id
                                     JOIN doctors d ON a.doctor_id = d.doctor_id
                                     WHERE p.patient_id = @search OR p.full_name LIKE @search_like";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", nameOrId);
                        cmd.Parameters.AddWithValue("@search_like", $"%{nameOrId}%");
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
                Logger.LogError(ex, $"Error searching appointments with search term: {nameOrId}");
                throw;
            }
        }

        public DataTable GetAll()
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT a.appointment_id AS 'Appointment ID', p.patient_id AS 'Patient ID', 
                                            p.full_name AS 'Patient Name', d.full_name AS 'Doctor Name', 
                                            a.appointment_date AS 'Date', a.time_slot AS 'Time Slot', 
                                            a.reason AS 'Reason', a.status AS 'Status', a.notes AS 'Notes'
                                     FROM appointments a
                                     JOIN patients p ON a.patient_id = p.patient_id
                                     JOIN doctors d ON a.doctor_id = d.doctor_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
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
                Logger.LogError(ex, "Error getting all appointments");
                throw;
            }
        }
    }
}
