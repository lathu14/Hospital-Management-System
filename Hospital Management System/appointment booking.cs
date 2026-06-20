using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System
{
    public partial class appoinment_booking : Form
    {
        private readonly AppointmentService _appointmentService = new AppointmentService();
        private readonly DoctorService _doctorService = new DoctorService();
        
        // Programmatically added controls
        private TextBox txtPatientId;
        private Label lblPatientId;

        public appoinment_booking()
        {
            InitializeComponent();
            
            // Programmatically inject Patient ID inputs into the UI layout
            InitializePatientIdControls();

            // Programmatically hook up event handlers
            this.Load += new EventHandler(this.appoinment_booking_Load);
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button11.Click += new EventHandler(this.button11_Click);
            this.button12.Click += new EventHandler(this.button12_Click);

            // Programmatically hook up sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void InitializePatientIdControls()
        {
            lblPatientId = new Label();
            lblPatientId.Text = "Patient ID";
            lblPatientId.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblPatientId.Location = new Point(570, 62);
            lblPatientId.Size = new Size(100, 23);

            txtPatientId = new TextBox();
            txtPatientId.Name = "txtPatientId";
            txtPatientId.Location = new Point(570, 88);
            txtPatientId.Size = new Size(218, 27);

            this.Controls.Add(lblPatientId);
            this.Controls.Add(txtPatientId);

            // Make sure these programmatically added controls are drawn on top
            lblPatientId.BringToFront();
            txtPatientId.BringToFront();
        }

        private void appoinment_booking_Load(object sender, EventArgs e)
        {
            LoadDoctors();
            PopulateTimeSlots();
            PopulateStatuses();
        }

        private void LoadDoctors()
        {
            try
            {
                DataTable dt = _doctorService.GetAllActive();
                comboBox1.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    comboBox1.Items.Add(new DoctorSelectItem
                    {
                        DoctorId = row["Doctor ID"].ToString(),
                        FullName = row["Full Name"].ToString(),
                        Specialization = row["Specialization"].ToString()
                    });
                }
                comboBox1.SelectedIndex = -1;
                textBox1.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading doctors in appointment booking");
            }
        }

        private void PopulateTimeSlots()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] {
                "09.00 a.m - 12.00p.m",
                "12.00 p.m - 03.00p.m",
                "03.00 p.m - 06.00p.m",
                "06.00 p.m - 09.00p.m"
            });
            comboBox2.SelectedIndex = -1;
        }

        private void PopulateStatuses()
        {
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(new string[] { "Pending", "Scheduled", "Completed", "Cancelled" });
            comboBox3.SelectedIndex = 1; // Default to Scheduled
        }

        // Selected doctor changed -> Auto-fill specialization
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DoctorSelectItem item)
            {
                textBox1.Text = item.Specialization;
            }
            else
            {
                textBox1.Clear();
            }
        }

        // BOOK Appointment
        private void button1_Click(object sender, EventArgs e)
        {
            string patientId = txtPatientId.Text.Trim();
            
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select an appointment time slot.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string doctorId = ((DoctorSelectItem)comboBox1.SelectedItem).DoctorId;
            DateTime date = dateTimePicker1.Value.Date;
            string timeSlot = comboBox2.SelectedItem.ToString();
            string reason = textBox2.Text.Trim();
            string status = comboBox3.SelectedItem?.ToString() ?? "Scheduled";
            string notes = textBox3.Text.Trim();

            var appt = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = date,
                TimeSlot = timeSlot,
                Reason = reason,
                Status = status,
                Notes = notes
            };

            var (ok, msg) = _appointmentService.Book(appt);

            if (ok)
            {
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // CLEAR Button
        private void button11_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtPatientId.Clear();
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = 1;
            textBox2.Clear();
            textBox3.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        // BACK Button
        private void button12_Click(object sender, EventArgs e)
        {
            login loginForm = new login();
            loginForm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }

        private class DoctorSelectItem
        {
            public string DoctorId { get; set; }
            public string FullName { get; set; }
            public string Specialization { get; set; }
            public override string ToString() => FullName;
        }
    }
}
