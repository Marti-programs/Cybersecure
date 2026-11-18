using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Cybersecure
{
    // Globalny filtr aktywności monitorujący mysz i klawiaturę w całym oknie
    public class MainFormActivityFilter : IMessageFilter
    {
        public event Action OnUserActivity;

        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_LBUTTONDOWN = 0x0201;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_KEYDOWN || m.Msg == WM_LBUTTONDOWN)
            {
                OnUserActivity?.Invoke();
            }
            return false;
        }
    }

    public partial class MainForm : Form
    {
        private string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Cybersecure;Trusted_Connection=True;";

        private System.Windows.Forms.Timer inactivityTimer;
        private int sessionTimeoutMinutes = 15;
        private int idleSecondsCounter = 0;
        private MainFormActivityFilter _activityFilter;

        public MainForm()
        {
            InitializeComponent();
            PokazNapisZalogowano();
            InitInactivityTimer(); 
        }

        private void InitInactivityTimer()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT SessionTimeoutMinutes FROM [SystemSettings] WHERE Id = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            sessionTimeoutMinutes = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch { /* Użyj domyślnego czasu w razie problemów z bazą */ }

            inactivityTimer = new System.Windows.Forms.Timer();
            inactivityTimer.Interval = 1000; 
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();

            _activityFilter = new MainFormActivityFilter();
            _activityFilter.OnUserActivity += ResetIdleTimer;
            Application.AddMessageFilter(_activityFilter);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_activityFilter != null)
            {
                Application.RemoveMessageFilter(_activityFilter);
            }
            base.OnFormClosed(e);
        }

        private void ResetIdleTimer()
        {
            idleSecondsCounter = 0;
        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            idleSecondsCounter++;
            int timeoutLimitSeconds = sessionTimeoutMinutes * 60;

            if (idleSecondsCounter >= timeoutLimitSeconds)
            {
                inactivityTimer.Stop();

                MessageBox.Show("Twoja sesja wygasła z powodu bezczynności.", "Wylogowanie automatyczne", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }

        private void PokazNapisZalogowano()
        {
            Label lblZalogowano = new Label();
            lblZalogowano.Text = "Zalogowano";
            lblZalogowano.Font = new Font("Segoe UI", 36, FontStyle.Bold);
            lblZalogowano.ForeColor = Color.Green;
            lblZalogowano.AutoSize = false;
            lblZalogowano.Dock = DockStyle.Fill;
            lblZalogowano.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(lblZalogowano);

            if (Controls.Contains(btnLogout))
            {
                btnLogout.BringToFront();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Czy na pewno chcesz się wylogować?", "Wylogowanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Retry; 
                this.Close();
            }
        }
    }
}