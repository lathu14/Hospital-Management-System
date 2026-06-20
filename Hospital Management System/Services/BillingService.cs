using System;
using System.Data;
using System.Collections.Generic;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class BillingService
    {
        private readonly BillingRepository _billingRepository = new BillingRepository();
        private readonly PatientRepository _patientRepository = new PatientRepository();
        private readonly AppointmentRepository _appointmentRepository = new AppointmentRepository();

        public (bool ok, string msg, Bill bill) GenerateBill(string patientId, string appointmentIdText)
        {
            if (string.IsNullOrWhiteSpace(patientId))
            {
                return (false, "Please enter a valid Patient ID.", null);
            }

            if (!_patientRepository.PatientIdExists(patientId))
            {
                return (false, $"Patient ID '{patientId}' is not registered.", null);
            }

            int? appointmentId = null;
            if (!string.IsNullOrWhiteSpace(appointmentIdText))
            {
                if (int.TryParse(appointmentIdText, out int apptId))
                {
                    appointmentId = apptId;
                }
                else
                {
                    return (false, "Appointment ID must be a valid number.", null);
                }
            }

            try
            {
                // Retrieve appointment to check reason or details for customization if needed
                decimal patientFee = 50.00m;
                decimal admissionFee = 0.00m;
                decimal consultationFee = 100.00m;
                decimal additionalCharges = 25.00m;

                if (appointmentId.HasValue)
                {
                    // Mock customization: if appointment ID is even, add some admission fee, etc.
                    if (appointmentId.Value % 2 == 0)
                    {
                        admissionFee = 150.00m;
                    }
                }

                decimal total = patientFee + admissionFee + consultationFee + additionalCharges;

                var bill = new Bill
                {
                    PatientId = patientId,
                    AppointmentId = appointmentId,
                    TotalAmount = total,
                    Status = "Unpaid",
                    CreatedAt = DateTime.Now
                };

                bill.Items.Add(new BillItem { ItemName = "Patient Fee", Quantity = 1, UnitPrice = patientFee });
                bill.Items.Add(new BillItem { ItemName = "Admission Fee", Quantity = 1, UnitPrice = admissionFee });
                bill.Items.Add(new BillItem { ItemName = "Consultation Fee", Quantity = 1, UnitPrice = consultationFee });
                bill.Items.Add(new BillItem { ItemName = "Additional Charges", Quantity = 1, UnitPrice = additionalCharges });

                _billingRepository.Create(bill);

                return (true, "Bill Generated Successfully", bill);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error generating bill for patient: {patientId}");
                return (false, "An error occurred while generating the bill.", null);
            }
        }

        public DataTable GetBillItems(int billId)
        {
            return _billingRepository.GetBillItems(billId);
        }
    }
}
