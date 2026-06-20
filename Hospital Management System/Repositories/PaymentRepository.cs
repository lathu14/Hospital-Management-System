using System;
using System.Data;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Models;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Repositories
{
    public class PaymentRepository
    {
        public void Create(Payment payment)
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
                            // 1. Insert Payment record
                            string queryPay = "INSERT INTO payments (bill_id, amount_paid, payment_method) VALUES (@bill_id, @amount, @method)";
                            using (var cmd = new MySqlCommand(queryPay, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@bill_id", payment.BillId);
                                cmd.Parameters.AddWithValue("@amount", payment.AmountPaid);
                                cmd.Parameters.AddWithValue("@method", payment.PaymentMethod);
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Update status of the associated bill to 'Paid'
                            string queryBill = "UPDATE bills SET status = 'Paid' WHERE bill_id = @bill_id";
                            using (var cmd = new MySqlCommand(queryBill, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@bill_id", payment.BillId);
                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
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
                Logger.LogError(ex, $"Error creating payment for bill ID: {payment.BillId}");
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
                    string query = @"SELECT p.payment_id AS 'Payment ID', p.bill_id AS 'Bill ID', 
                                            p.amount_paid AS 'Amount Paid', p.payment_method AS 'Method', 
                                            p.payment_date AS 'Date', b.status AS 'Bill Status',
                                            pat.full_name AS 'Patient Name'
                                     FROM payments p
                                     JOIN bills b ON p.bill_id = b.bill_id
                                     JOIN patients pat ON b.patient_id = pat.patient_id
                                     ORDER BY p.payment_date DESC";
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
                Logger.LogError(ex, "Error getting all payments");
                throw;
            }
        }
    }
}
