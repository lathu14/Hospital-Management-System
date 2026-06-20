using System;
using System.Drawing;
using System.Windows.Forms;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;
using MySql.Data.MySqlClient;

namespace Hospital_Management_System
{
    public partial class patient_registration : Form
    {
        private readonly PatientService _patientService = new PatientService();

        public patient_registration()
        {
            InitializeComponent();

            // Programmatically hook up sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void patient_registration_Load(object sender, EventArgs e)
        {
            GeneratePatientID();
            PopulateBloodGroups();
        }

        private void GeneratePatientID()
        {
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM patients";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        maskedTextBox1.Text = "P" + (count + 1).ToString("D4"); // P0001, P0002 …
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error auto-generating patient ID");
                maskedTextBox1.Text = "P0001";
            }
        }

        private void PopulateBloodGroups()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" });
            comboBox1.SelectedIndex = -1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void pictureBox12_Click(object sender, EventArgs e) { }

        private void pictureBox12_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select Patient Photo";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox12.Image = Image.FromFile(ofd.FileName);
                    pictureBox12.Tag = ofd.FileName; // Store path
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }

        private void ClearForm()
        {
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1;
            maskedTextBox6.Clear();
            maskedTextBox7.Clear();
            maskedTextBox8.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        // button5 — Add Doctor
        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigate to Add Doctor page.", "Navigation");
            // add_doctor frm = new add_doctor();
            // frm.Show();
            // this.Hide();
        }

        // button6 — Appointment Booking
        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigate to Appointment Booking page.", "Navigation");
            // appointment_booking frm = new appointment_booking();
            // frm.Show();
            // this.Hide();
        }

        // button7 — View Appointments
        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigate to View Appointments page.", "Navigation");
            // view_appointments frm = new view_appointments();
            // frm.Show();
            // this.Hide();
        }

        // button8 — Doctor Schedule
        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigate to Doctor Schedule page.", "Navigation");
            // doctor_schedule frm = new doctor_schedule();
            // frm.Show();
            // this.Hide();
        }

        // button9 — Billing
        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigate to Billing page.", "Navigation");
            // billing frm = new billing();
            // frm.Show();
            // this.Hide();
        }

        // button10 — Payments
        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigate to Payments page.", "Navigation");
            // payments frm = new payments();
            // frm.Show();
            // this.Hide();
        }

        // CLEAR Form — bound by Designer
        private void button12_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all fields?", "Confirm Clear",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearForm();
            }
        }

        // SAVE Patient — bound by Designer
        private void button11_Click_1(object sender, EventArgs e)
        {
            string patientId = maskedTextBox1.Text.Trim();
            string fullName  = maskedTextBox2.Text.Trim();
            string dobText   = maskedTextBox3.Text.Trim();
            string ageText   = maskedTextBox4.Text.Trim();
            string gender    = radioButton1.Checked ? "Male" : (radioButton2.Checked ? "Female" : "");
            string bloodGroup = comboBox1.SelectedItem?.ToString() ?? "";
            string phone     = maskedTextBox6.Text.Trim();
            string email     = maskedTextBox7.Text.Trim();
            string address   = maskedTextBox8.Text.Trim();
            string photoPath = pictureBox12.Tag?.ToString() ?? "";

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(dobText) ||
                string.IsNullOrEmpty(gender)   || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Please enter all required fields (Full Name, DOB, Gender, Phone).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DateTime.TryParse(dobText, out DateTime dob))
            {
                MessageBox.Show("Please enter a valid Date of Birth (e.g. 1990-06-15).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? age = null;
            if (!string.IsNullOrEmpty(ageText) && int.TryParse(ageText, out int a))
                age = a;

            var patient = new Patient
            {
                PatientId   = patientId,
                FullName    = fullName,
                DateOfBirth = dob,
                Age         = age,
                Gender      = gender,
                BloodGroup  = bloodGroup,
                PhoneNumber = phone,
                Email       = email,
                Address     = address,
                PhotoPath   = photoPath
            };

            var (ok, msg) = _patientService.Register(patient);

            if (ok)
            {
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                GeneratePatientID();
            }
            else
            {
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
