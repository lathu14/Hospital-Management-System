using System;
using System.Data;
using System.Windows.Forms;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System
{
    public partial class doctor_schedule : Form
    {
        private readonly DoctorScheduleService _scheduleService = new DoctorScheduleService();
        private readonly DoctorService _doctorService = new DoctorService();
        private bool _isUpdatingUi = false;

        public doctor_schedule()
        {
            InitializeComponent();

            // Programmatically hook up event handlers
            this.Load += new EventHandler(this.doctor_schedule_Load);
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.DoctorOrDate_Changed);
            this.dateTimePicker1.ValueChanged += new EventHandler(this.DoctorOrDate_Changed);

            this.radioButton1.Click += new EventHandler(this.SlotRadio_Click);
            this.radioButton2.Click += new EventHandler(this.SlotRadio_Click);
            this.radioButton3.Click += new EventHandler(this.SlotRadio_Click);
            this.radioButton4.Click += new EventHandler(this.SlotRadio_Click);

            // Programmatically hook up sidebar navigation
            NavigationHelper.SetupNavigation(this, this.panel1);
        }

        private void doctor_schedule_Load(object sender, EventArgs e)
        {
            LoadDoctors();
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
                        FullName = row["Full Name"].ToString()
                    });
                }
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading doctors in schedule");
            }
        }

        private void DoctorOrDate_Changed(object sender, EventArgs e)
        {
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            if (comboBox1.SelectedItem == null) return;
            var doctor = (DoctorSelectItem)comboBox1.SelectedItem;
            DateTime date = dateTimePicker1.Value.Date;

            try
            {
                _isUpdatingUi = true;
                DataTable dt = _scheduleService.GetSchedule(doctor.DoctorId, date);
                dataGridView1.DataSource = dt;

                // Reset radio buttons selection
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;

                // Highlight existing slots in radio buttons
                foreach (DataRow row in dt.Rows)
                {
                    string slot = row["Time Slot"].ToString();
                    string status = row["Status"].ToString();
                    
                    if (slot.Contains("09.00 a.m"))
                    {
                        radioButton1.Checked = true;
                    }
                    else if (slot.Contains("12.00 p.m"))
                    {
                        radioButton2.Checked = true;
                    }
                    else if (slot.Contains("03.00 p.m"))
                    {
                        radioButton4.Checked = true; // Wait: radioButton4 is 03.00 p.m - 06.00p.m in Designer!
                    }
                    else if (slot.Contains("06.00 p.m"))
                    {
                        radioButton3.Checked = true; // radioButton3 is 06.00 p.m - 09.00p.m in Designer!
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error loading schedule for doctor: {doctor.DoctorId}");
            }
            finally
            {
                _isUpdatingUi = false;
            }
        }

        private void SlotRadio_Click(object sender, EventArgs e)
        {
            if (_isUpdatingUi) return;
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a doctor first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var radio = sender as RadioButton;
            if (radio == null || !radio.Checked) return;

            var doctor = (DoctorSelectItem)comboBox1.SelectedItem;
            DateTime date = dateTimePicker1.Value.Date;
            string timeSlot = radio.Text;

            // Ask if user wants to add/configure this slot as Available
            var result = MessageBox.Show($"Do you want to configure '{timeSlot}' as an Available schedule slot for Dr. {doctor.FullName} on {date:yyyy-MM-dd}?",
                                         "Configure Schedule Slot", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var schedule = new DoctorSchedule
                {
                    DoctorId = doctor.DoctorId,
                    Date = date,
                    TimeSlot = timeSlot,
                    Status = "Available"
                };

                var (ok, msg) = _scheduleService.AddSlot(schedule);
                MessageBox.Show(msg, ok ? "Success" : "Error", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                LoadSchedule();
            }
            else
            {
                // Revert check state by reloading
                LoadSchedule();
            }
        }

        private class DoctorSelectItem
        {
            public string DoctorId { get; set; }
            public string FullName { get; set; }
            public override string ToString() => FullName;
        }
    }
}
