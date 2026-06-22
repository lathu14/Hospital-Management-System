using System;
using System.Data;
using System.Windows.Forms;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;
using MySql.Data.MySqlClient;

namespace Hospital_Management_System
{
    public partial class billing : Form
    {
        private readonly BillingService _billingService = new BillingService();
        private readonly PatientService _patientService = new PatientService();

        public billing()
        {
            InitializeComponent();

            // Ensure the form always opens fully visible (opacity = 1)
            this.Opacity = 1D;

            // Programmatically hook up event handlers
            this.button1.Click += new EventHandler(this.button1_Click);
            this.textBox1.Leave += new EventHandler(this.PatientId_Leave);

            // Programmatically hook up sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);this.panel1.BackColor = Color.DarkSlateGray;
        }

        // Auto-fill patient name when Patient ID is entered
        private void PatientId_Leave(object sender, EventArgs e)
        {
            string patientId = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(patientId)) return;

            try
            {
                DataTable dt = _patientService.Search(patientId);
                if (dt.Rows.Count > 0)
                {
                    textBox2.Text = dt.Rows[0]["Full Name"].ToString();
                }
                else
                {
                    textBox2.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error searching patient ID: {patientId} on leave");
            }
        }

        // GENERATE BILL Button Click
        private void button1_Click(object sender, EventArgs e)
        {
            string patientId = textBox1.Text.Trim();
            string appointmentIdText = textBox4.Text.Trim();

            var (ok, msg, bill) = _billingService.GenerateBill(patientId, appointmentIdText);

            if (ok && bill != null)
            {
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Display total
                lbltotal.Text = $"${bill.TotalAmount:0.00}";

                // Load bill items into DataGridView
                DataTable dtItems = _billingService.GetBillItems(bill.BillId);
                dataGridView1.DataSource = dtItems;
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label4_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
    }
}
