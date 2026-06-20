using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System
{
    public partial class payment : Form
    {
        public payment()
        {
            InitializeComponent();
            
            // Setup navigation for the sidebar panel
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void payment_Load(object sender, EventArgs e)
        {
            LoadPaymentHistory();
            LoadUnpaidBills();
        }

        private void LoadPaymentHistory()
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
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading payment history");
                MessageBox.Show("Error loading payment history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUnpaidBills()
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT b.bill_id, pat.full_name, b.total_amount FROM bills b JOIN patients pat ON b.patient_id = pat.patient_id WHERE b.status != 'Paid'";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            comboBoxBill.Items.Clear();
                            while (reader.Read())
                            {
                                comboBoxBill.Items.Add(new BillSelectItem
                                {
                                    BillId = Convert.ToInt32(reader["bill_id"]),
                                    PatientName = reader["full_name"].ToString(),
                                    TotalAmount = Convert.ToDecimal(reader["total_amount"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading unpaid bills");
            }
        }

        private void comboBoxBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBill.SelectedItem is BillSelectItem item)
            {
                textBoxAmount.Text = item.TotalAmount.ToString("0.00");
            }
        }

        private void buttonPay_Click(object sender, EventArgs e)
        {
            if (comboBoxBill.SelectedItem == null)
            {
                MessageBox.Show("Please select a Bill.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBoxAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive payment amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedBill = (BillSelectItem)comboBoxBill.SelectedItem;

            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert Payment
                            string payQuery = "INSERT INTO payments (bill_id, amount_paid, payment_method) VALUES (@bill_id, @amount, @method)";
                            using (var cmd = new MySqlCommand(payQuery, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@bill_id", selectedBill.BillId);
                                cmd.Parameters.AddWithValue("@amount", amount);
                                cmd.Parameters.AddWithValue("@method", comboBoxMethod.SelectedItem.ToString());
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Update Bill status
                            string billQuery = "UPDATE bills SET status = 'Paid' WHERE bill_id = @bill_id";
                            using (var cmd = new MySqlCommand(billQuery, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@bill_id", selectedBill.BillId);
                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
                            MessageBox.Show("Payment Successful! Bill status updated to Paid.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Reset inputs
                            textBoxAmount.Clear();
                            comboBoxBill.SelectedIndex = -1;
                            comboBoxMethod.SelectedIndex = -1;

                            // Reload
                            LoadPaymentHistory();
                            LoadUnpaidBills();
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
                Logger.LogError(ex, "Error processing payment");
                MessageBox.Show("Payment Failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class BillSelectItem
        {
            public int BillId { get; set; }
            public string PatientName { get; set; }
            public decimal TotalAmount { get; set; }

            public override string ToString()
            {
                return $"Bill #{BillId} - {PatientName} (${TotalAmount:F2})";
            }
        }
    }
}
