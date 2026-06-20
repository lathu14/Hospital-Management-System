using System;
using System.Data;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class DoctorRepository
    {
        public bool DoctorIdExists(string doctorId)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM doctors WHERE doctor_id = @doctor_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctor_id", doctorId);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error checking doctor existence: {doctorId}");
                throw;
            }
        }

        public bool MedicalLicenseNumberExists(string licenseNum)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM doctors WHERE medical_license_number = @license";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@license", licenseNum);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error checking medical license existence: {licenseNum}");
                throw;
            }
        }

        public void Create(Doctor doctor)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO doctors (doctor_id, full_name, email, phone_number, gender, specialization, qualification, status, address, medical_license_number, available_days)
                                     VALUES (@doctor_id, @full_name, @email, @phone, @gender, @spec, @qual, @status, @address, @license, @days)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctor_id", doctor.DoctorId);
                        cmd.Parameters.AddWithValue("@full_name", doctor.FullName);
                        cmd.Parameters.AddWithValue("@email", doctor.Email);
                        cmd.Parameters.AddWithValue("@phone", doctor.PhoneNumber);
                        cmd.Parameters.AddWithValue("@gender", doctor.Gender);
                        cmd.Parameters.AddWithValue("@spec", doctor.Specialization);
                        cmd.Parameters.AddWithValue("@qual", doctor.Qualification);
                        cmd.Parameters.AddWithValue("@status", doctor.Status);
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(doctor.Address) ? DBNull.Value : (object)doctor.Address);
                        cmd.Parameters.AddWithValue("@license", doctor.MedicalLicenseNumber);
                        cmd.Parameters.AddWithValue("@days", doctor.AvailableDays);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error saving doctor: {doctor.DoctorId}");
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
                    string query = "SELECT doctor_id AS 'Doctor ID', full_name AS 'Full Name', specialization AS 'Specialization' FROM doctors WHERE status = 'Active'";
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
                Logger.LogError(ex, "Error getting all active doctors");
                throw;
            }
        }
    }
}
