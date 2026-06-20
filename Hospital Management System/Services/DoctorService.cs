using System;
using System.Data;
using System.Text.RegularExpressions;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepository = new DoctorRepository();

        public (bool ok, string msg) AddDoctor(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.DoctorId) ||
                string.IsNullOrWhiteSpace(doctor.FullName) ||
                string.IsNullOrWhiteSpace(doctor.Email) ||
                string.IsNullOrWhiteSpace(doctor.PhoneNumber) ||
                string.IsNullOrWhiteSpace(doctor.Gender) ||
                string.IsNullOrWhiteSpace(doctor.Specialization) ||
                string.IsNullOrWhiteSpace(doctor.Qualification) ||
                string.IsNullOrWhiteSpace(doctor.MedicalLicenseNumber) ||
                string.IsNullOrWhiteSpace(doctor.AvailableDays))
            {
                return (false, "Please fill in all fields.");
            }

            if (!Regex.IsMatch(doctor.PhoneNumber, @"^\+?[0-9]{7,15}$"))
            {
                return (false, "Please enter a valid phone number (7-15 digits).");
            }

            if (!Regex.IsMatch(doctor.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return (false, "Please enter a valid email address.");
            }

            try
            {
                if (_doctorRepository.DoctorIdExists(doctor.DoctorId))
                {
                    return (false, $"Doctor ID '{doctor.DoctorId}' already exists.");
                }

                if (_doctorRepository.MedicalLicenseNumberExists(doctor.MedicalLicenseNumber))
                {
                    return (false, $"Medical License Number '{doctor.MedicalLicenseNumber}' is already registered.");
                }

                _doctorRepository.Create(doctor);
                return (true, "Doctor Added Successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error adding doctor: {doctor.DoctorId}");
                return (false, "An error occurred while saving doctor information.");
            }
        }

        public DataTable GetAllActive()
        {
            return _doctorRepository.GetAll();
        }
    }
}
