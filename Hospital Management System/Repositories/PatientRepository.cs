using System;
using System.Data;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class PatientRepository
    {
        public bool PatientIdExists(string patientId)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM patients WHERE patient_id = @patient_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@patient_id", patientId);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error checking patient existence: {patientId}");
                throw;
            }
        }

        public void Create(Patient patient)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO patients (patient_id, full_name, date_of_birth, age, gender, blood_group, phone_number, email, address, photo_path)
                                     VALUES (@patient_id, @full_name, @date_of_birth, @age, @gender, @blood_group, @phone_number, @email, @address, @photo_path)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@patient_id", patient.PatientId);
                        cmd.Parameters.AddWithValue("@full_name", patient.FullName);
                        cmd.Parameters.AddWithValue("@date_of_birth", patient.DateOfBirth);
                        cmd.Parameters.AddWithValue("@age", patient.Age.HasValue ? (object)patient.Age.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@gender", patient.Gender);
                        cmd.Parameters.AddWithValue("@blood_group", string.IsNullOrEmpty(patient.BloodGroup) ? DBNull.Value : (object)patient.BloodGroup);
                        cmd.Parameters.AddWithValue("@phone_number", patient.PhoneNumber);
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(patient.Email) ? DBNull.Value : (object)patient.Email);
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(patient.Address) ? DBNull.Value : (object)patient.Address);
                        cmd.Parameters.AddWithValue("@photo_path", string.IsNullOrEmpty(patient.PhotoPath) ? DBNull.Value : (object)patient.PhotoPath);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error saving patient: {patient.PatientId}");
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
                    string query = @"SELECT patient_id AS 'Patient ID', full_name AS 'Full Name', 
                                            date_of_birth AS 'DOB', age AS 'Age', gender AS 'Gender', 
                                            blood_group AS 'Blood Group', phone_number AS 'Phone', 
                                            email AS 'Email', address AS 'Address'
                                     FROM patients 
                                     WHERE patient_id = @search OR full_name LIKE @search_like";
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
                Logger.LogError(ex, $"Error searching patients with search term: {nameOrId}");
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
                    string query = @"SELECT patient_id AS 'Patient ID', full_name AS 'Full Name', 
                                            date_of_birth AS 'DOB', age AS 'Age', gender AS 'Gender', 
                                            blood_group AS 'Blood Group', phone_number AS 'Phone', 
                                            email AS 'Email', address AS 'Address'
                                     FROM patients";
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
                Logger.LogError(ex, "Error getting all patients");
                throw;
            }
        }
    }
}
