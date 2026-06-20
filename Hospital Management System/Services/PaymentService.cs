using System;
using System.Data;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository = new PaymentRepository();

        public (bool ok, string msg) ProcessPayment(Payment payment)
        {
            if (payment.BillId <= 0)
            {
                return (false, "Please select a valid Bill.");
            }

            if (payment.AmountPaid <= 0)
            {
                return (false, "Payment amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(payment.PaymentMethod))
            {
                return (false, "Please select a payment method.");
            }

            try
            {
                _paymentRepository.Create(payment);
                return (true, "Payment Successful! Bill status updated to Paid.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error processing payment for bill ID: {payment.BillId}");
                return (false, "An error occurred while processing the payment.");
            }
        }

        public DataTable GetAll()
        {
            return _paymentRepository.GetAll();
        }
    }
}
