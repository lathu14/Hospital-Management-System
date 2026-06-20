using System;
using System.Windows.Forms;

namespace Hospital_Management_System.Helpers
{
    public static class NavigationHelper
    {
        public static void SetupNavigation(Form currentForm, Panel sidebarPanel)
        {
            if (sidebarPanel == null) return;

            foreach (Control control in sidebarPanel.Controls)
            {
                if (control is Button btn)
                {
                    btn.Click += (sender, e) =>
                    {
                        string btnText = btn.Text.Trim();
                        Form targetForm = null;

                        // Check role/auth restriction if needed. For now, open matching forms.
                        switch (btnText)
                        {
                            case "Login":
                                targetForm = new login();
                                break;
                            case "Patient Registration":
                                targetForm = new patient_registration();
                                break;
                            case "Patient Details":
                                targetForm = new patient_details();
                                break;
                            case "Doctor Registration":
                                targetForm = new Add_doctor();
                                break;
                            case "Doctor Schedule":
                                targetForm = new doctor_schedule();
                                break;
                            case "Appointment Booking":
                                targetForm = new appoinment_booking();
                                break;
                            case "View Appointments":
                                targetForm = new view_appointments();
                                break;
                            case "Billing":
                                targetForm = new billing();
                                break;
                            case "Payments":
                                targetForm = new payment();
                                break;
                        }

                        if (targetForm != null)
                        {
                            targetForm.Show();
                            currentForm.Hide();
                        }
                    };
                }
            }
        }
    }
}
