using System;
using System.Data;
using System.Windows.Forms;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System
{
    public partial class view_appointments : Form
    {
        private readonly AppointmentService _appointmentService = new AppointmentService();

        public view_appointments()
        {
            InitializeComponent();

            // Programmatically hook up event handlers
            this.Load += new EventHandler(this.view_appointments_Load);
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button12.Click += new EventHandler(this.button12_Click);

            // Programmatically hook up sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void view_appointments_Load(object sender, EventArgs e)
        {
            LoadAppointments("");
        }

        private void LoadAppointments(string query)
        {
            try
            {
                DataTable dt = _appointmentService.Search(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SEARCH Button Click
        private void button1_Click(object sender, EventArgs e)
        {
            string query = textBox5.Text.Trim();
            LoadAppointments(query);
        }

        // ADD APPOINTMENT Button Click (opens appointment booking form)
        private void button12_Click(object sender, EventArgs e)
        {
            appoinment_booking bookingForm = new appoinment_booking();
            bookingForm.Show();
            this.Hide();
        }
    }
}
