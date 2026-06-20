using System;
using System.Data;
using System.Text.RegularExpressions;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository = new PatientRepository();

        public (bool ok, string msg) Register(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.PatientId) ||
                string.IsNullOrWhiteSpace(patient.FullName) ||
                string.IsNullOrWhiteSpace(patient.Gender) ||
                string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                return (false, "Please fill in all required fields (ID, Full Name, Gender, Phone).");
            }

            if (!Regex.IsMatch(patient.PhoneNumber, @"^\+?[0-9]{7,15}$"))
            {
                return (false, "Please enter a valid phone number (7-15 digits).");
            }

            if (!string.IsNullOrWhiteSpace(patient.Email) && !Regex.IsMatch(patient.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return (false, "Please enter a valid email address.");
            }

            try
            {
                if (_patientRepository.PatientIdExists(patient.PatientId))
                {
                    return (false, $"Patient ID '{patient.PatientId}' is already registered.");
                }

                _patientRepository.Create(patient);
                return (true, "Patient Registered Successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error registering patient: {patient.PatientId}");
                return (false, "An error occurred while saving patient information.");
            }
        }

        public DataTable Search(string nameOrId)
        {
            if (string.IsNullOrWhiteSpace(nameOrId))
            {
                return _patientRepository.GetAll();
            }
            return _patientRepository.Search(nameOrId);
        }

        public DataTable GetAll()
        {
            return _patientRepository.GetAll();
        }
    }
}
