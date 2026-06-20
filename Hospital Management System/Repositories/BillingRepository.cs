using System;
using System.Data;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class BillingRepository
    {
        public void Create(Bill bill)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert into bills
                            string queryBill = @"INSERT INTO bills (appointment_id, patient_id, total_amount, status)
                                                 VALUES (@appointment_id, @patient_id, @total, @status);
                                                 SELECT LAST_INSERT_ID();";
                            int billId = 0;
                            using (var cmd = new MySqlCommand(queryBill, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@appointment_id", bill.AppointmentId.HasValue ? (object)bill.AppointmentId.Value : DBNull.Value);
                                cmd.Parameters.AddWithValue("@patient_id", bill.PatientId);
                                cmd.Parameters.AddWithValue("@total", bill.TotalAmount);
                                cmd.Parameters.AddWithValue("@status", bill.Status);
                                billId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 2. Insert into bill_items
                            foreach (var item in bill.Items)
                            {
                                string queryItem = @"INSERT INTO bill_items (bill_id, item_name, quantity, unit_price)
                                                     VALUES (@bill_id, @name, @qty, @price)";
                                using (var cmd = new MySqlCommand(queryItem, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@bill_id", billId);
                                    cmd.Parameters.AddWithValue("@name", item.ItemName);
                                    cmd.Parameters.AddWithValue("@qty", item.Quantity);
                                    cmd.Parameters.AddWithValue("@price", item.UnitPrice);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            bill.BillId = billId;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error creating bill for patient: {bill.PatientId}");
                throw;
            }
        }

        public DataTable GetBillItems(int billId)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT item_name AS 'Item Description', quantity AS 'Quantity', 
                                            unit_price AS 'Unit Price', subtotal AS 'Subtotal'
                                     FROM bill_items
                                     WHERE bill_id = @bill_id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bill_id", billId);
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
                Logger.LogError(ex, $"Error fetching bill items for bill ID: {billId}");
                throw;
            }
        }
    }
}
