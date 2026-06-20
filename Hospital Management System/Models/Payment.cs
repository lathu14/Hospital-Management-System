using System;

namespace Hospital_Management_System.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
