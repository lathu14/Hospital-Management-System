using System;
using System.Windows.Forms;
using Hospital_Management_System.Services;

namespace Hospital_Management_System
{
    public partial class login : Form
    {
        private readonly AuthService _authService = new AuthService();

        public login()
        {
            InitializeComponent();

            // Programmatically hook up event handlers
            this.Clear.Click += new EventHandler(this.Clear_Click);
            this.button2.Click += new EventHandler(this.SignUp_Click);

            // Mask the password text input characters
            this.Password.UseSystemPasswordChar = true;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void timer1_Tick(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = UserName.Text.Trim();
            string password = Password.Text.Trim();

            var (ok, msg, role) = _authService.Login(username, password);

            if (ok)
            {
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                patient_registration registrationForm = new patient_registration();
                registrationForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(msg, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Password.Clear();
                Password.Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        // Clear button event handler to reset inputs
        private void Clear_Click(object sender, EventArgs e)
        {
            UserName.Clear();
            Password.Clear();
            UserName.Focus();
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            signup signupForm = new signup();
            signupForm.Show();
            this.Hide();
        }

        private void Clear_Click_1(object sender, EventArgs e) { }
    }
}
