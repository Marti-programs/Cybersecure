using System;
using System.Windows.Forms;
using Cybersecure.Services;

namespace Cybersecure
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool logoutRequested = true;

            while (logoutRequested)
            {
                logoutRequested = false;

                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK && loginForm.IsAuthenticated)
                    {
                        int userId = loginForm.LoggedInUserId;
                        string permissions = loginForm.LoggedInPermissions;

                        if (loginForm.IsFirstLogin)
                        {
                            MessageBox.Show("Twoje hasło wymaga zmiany przed kontynuowaniem pracy.", "Bezpieczeństwo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            using (var changePasswordForm = new ChangePasswordForm(userId))
                            {
                                if (changePasswordForm.ShowDialog() != DialogResult.OK)
                                {
                                    return; 
                                }
                            }
                        }

                        bool isAdmin = !string.IsNullOrEmpty(permissions) &&
                                       permissions.IndexOf("admin", StringComparison.OrdinalIgnoreCase) >= 0;

                        Form mainForm = isAdmin ? (Form)new AdminPanelForm() : new MainForm();

                        Application.Run(mainForm);

                        if (mainForm.DialogResult == DialogResult.Retry)
                        {
                            logoutRequested = true;
                        }
                    }
                }
            }
        }
    }
}