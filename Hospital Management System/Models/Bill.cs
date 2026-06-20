using System;
using System.Collections.Generic;

namespace Hospital_Management_System.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public int? AppointmentId { get; set; }
        public string PatientId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public List<BillItem> Items { get; set; } = new List<BillItem>();
    }

    public class BillItem
    {
        public int ItemId { get; set; }
        public int BillId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
