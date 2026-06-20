using System;
using System.Windows.Forms;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;

namespace Hospital_Management_System
{
    public partial class signup : Form
    {
        private readonly AuthService _authService = new AuthService();

        public signup()
        {
            InitializeComponent();
            
            // Programmatically hook up event handlers
            this.Clear.Click += new EventHandler(this.SignUp_Click);
            this.button2.Click += new EventHandler(this.SignIn_Click);
            
            this.textBox5.UseSystemPasswordChar = true;
            this.textBox4.UseSystemPasswordChar = true;
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            string firstName = UserName.Text.Trim();
            string lastName = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string phone = textBox3.Text.Trim();
            string password = textBox5.Text.Trim();
            string confirmPassword = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Construct username as firstname_lastname (lowercased)
            string username = $"{firstName}_{lastName}".ToLower().Replace(" ", "");

            User newUser = new User
            {
                Username = username,
                Role = "Receptionist" // Default role for self-registration
            };

            var (ok, msg) = _authService.Register(newUser, email, password, confirmPassword);

            if (ok)
            {
                MessageBox.Show(msg + $"\nYour Username for Login is: {username}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                login loginForm = new login();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(msg, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SignIn_Click(object sender, EventArgs e)
        {
            login loginForm = new login();
            loginForm.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}
