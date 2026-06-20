using System;
using System.Windows.Forms;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;
using MySql.Data.MySqlClient;

namespace Hospital_Management_System
{
    public partial class Add_doctor : Form
    {
        private readonly DoctorService _doctorService = new DoctorService();

        public Add_doctor()
        {
            InitializeComponent();
            
            // Programmatically hook up event handlers
            this.button11.Click += new EventHandler(this.button11_Click);
            this.button12.Click += new EventHandler(this.button12_Click);
            this.Load += new EventHandler(this.Add_doctor_Load);

            // Setup dynamic sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void Add_doctor_Load(object sender, EventArgs e)
        {
            PopulateSpecializations();
            PopulateStatuses();
            GenerateDoctorID();
        }

        private void PopulateSpecializations()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] {
                "General Physician", "Surgeon", "Cardiologist", "Neurologist",
                "Orthopedic Surgeon", "Pediatrician", "Gynecologist", "Dermatologist",
                "ENT Specialist", "Psychiatrist", "Ophthalmologist", "Dentist"
            });
            comboBox1.SelectedIndex = -1;
        }

        private void PopulateStatuses()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] { "Active", "Inactive" });
            comboBox2.SelectedIndex = 0; // Default to Active
        }

        private void GenerateDoctorID()
        {
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM doctors";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        maskedTextBox1.Text = "D" + (count + 1).ToString("D4"); // D0001, D0002 …
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error auto-generating doctor ID");
                maskedTextBox1.Text = "D0001";
            }
        }

        // SAVE Doctor Details
        private void button11_Click(object sender, EventArgs e)
        {
            string doctorId = maskedTextBox1.Text.Trim();
            string fullName = maskedTextBox2.Text.Trim();
            string email = maskedTextBox3.Text.Trim();
            string phone = maskedTextBox4.Text.Trim();
            string gender = radioButton1.Checked ? "Male" : (radioButton2.Checked ? "Female" : "");
            string specialization = comboBox1.SelectedItem?.ToString() ?? "";
            string qualification = maskedTextBox5.Text.Trim();
            string status = comboBox2.SelectedItem?.ToString() ?? "Active";
            string address = maskedTextBox6.Text.Trim();
            string license = maskedTextBox7.Text.Trim();
            string availableDays = radioButton3.Checked ? "Week Days" : (radioButton4.Checked ? "Weekends" : "");

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Please Enter Doctor Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBox2.Focus();
                return;
            }

            if (string.IsNullOrEmpty(doctorId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(specialization) || string.IsNullOrEmpty(qualification) ||
                string.IsNullOrEmpty(license) || string.IsNullOrEmpty(availableDays))
            {
                MessageBox.Show("Please fill in all doctor details fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = fullName,
                Email = email,
                PhoneNumber = phone,
                Gender = gender,
                Specialization = specialization,
                Qualification = qualification,
                Status = status,
                Address = address,
                MedicalLicenseNumber = license,
                AvailableDays = availableDays
            };

            var (ok, msg) = _doctorService.AddDoctor(doctor);

            if (ok)
            {
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                GenerateDoctorID();
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // CLEAR Button Click
        private void button12_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            maskedTextBox5.Clear();
            maskedTextBox6.Clear();
            maskedTextBox7.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = 0;
        }

        private void pictureBox11_Click(object sender, EventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}
