using System;
using System.Data;
using System.Windows.Forms;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System
{
    public partial class patient_details : Form
    {
        private readonly PatientService _patientService = new PatientService();

        public patient_details()
        {
            InitializeComponent();
            
            // Programmatically hook up event handlers to ensure they are fired
            this.Load += new EventHandler(this.patient_details_Load);
            this.button1.Click += new EventHandler(this.button1_Click);

            // Programmatically hook up sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void patient_details_Load(object sender, EventArgs e)
        {
            // Initially load all patients
            LoadPatients("");
        }

        private void LoadPatients(string query)
        {
            try
            {
                DataTable dt = _patientService.Search(query);
                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0 && !string.IsNullOrEmpty(query))
                {
                    MessageBox.Show("No Patient Found", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patients: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }

        // Search Button click
        private void button1_Click(object sender, EventArgs e)
        {
            string query = textBox5.Text.Trim();
            LoadPatients(query);
        }

        // Add New Patient Button click
        private void button12_Click(object sender, EventArgs e)
        {
            patient_registration regForm = new patient_registration();
            regForm.Show();
            this.Hide();
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e) { }
    }
}
