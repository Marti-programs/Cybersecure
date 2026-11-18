using Cybersecure.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Cybersecure
{
    // Globalny filtr aktywności monitorujący mysz i klawiaturę w całym oknie
    public class ActivityFilter : IMessageFilter
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

    public partial class AdminPanelForm : Form
    {
        private string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Cybersecure;Trusted_Connection=True;";


        private System.Windows.Forms.Timer inactivityTimer;
        private int sessionTimeoutMinutes = 15; // Domyślnie
        private int idleSecondsCounter = 0;
        private ActivityFilter _activityFilter;

        public AdminPanelForm()
        {
            InitializeComponent();
            LoadRolesIntoComboBox();
            LoadUsers();
            LoadSystemSettings();
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
            catch { /* Użyj domyślnego w razie błędu */ }

            inactivityTimer = new System.Windows.Forms.Timer();
            inactivityTimer.Interval = 1000;
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();

            _activityFilter = new ActivityFilter();
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

        private void LoadRolesIntoComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Id, RoleName FROM [Roles]";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            cmbRole.DataSource = dt;
                            cmbRole.DisplayMember = "RoleName";
                            cmbRole.ValueMember = "Id";      
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania ról: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT u.Id, u.Username, r.RoleName AS Role, u.IsBlocked, u.IsFirstLogin, u.TempPasswordExpiry, u.PasswordNeverExpires 
                                   FROM [Users] u 
                                   JOIN [Roles] r ON u.RoleId = r.Id";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvUsers.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania użytkowników: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSystemSettings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PolicyEnabled, RequireLevenshtein, ExpiryDays, SessionTimeoutMinutes FROM [SystemSettings] WHERE Id = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                chkPolicyEnabled.Checked = reader.GetBoolean(0);
                                chkRequireLevenshtein.Checked = reader.GetBoolean(1);
                                txtExpiryDays.Text = reader.GetInt32(2).ToString();
                                txtSessionTimeout.Text = reader.GetInt32(3).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania ustawień: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();

            if (string.IsNullOrEmpty(username) || cmbRole.SelectedValue == null)
            {
                MessageBox.Show("Podaj nazwę użytkownika i wybierz rolę!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedRoleId = Convert.ToInt32(cmbRole.SelectedValue);

            try
            {
                string tempPassword = SecurityService.GenerateTemporaryPassword();
                var (hash, salt) = SecurityService.HashPassword(tempPassword);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO [Users] (Username, PasswordHash, Salt, RoleId, IsBlocked, IsFirstLogin, PasswordChangedDate, TempPasswordExpiry, PasswordNeverExpires) 
                                   VALUES (@username, @hash, @salt, @roleId, 0, 1, GETDATE(), DATEADD(hour, 4, GETDATE()), 0)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@hash", hash);
                        cmd.Parameters.AddWithValue("@salt", salt);
                        cmd.Parameters.AddWithValue("@roleId", selectedRoleId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Użytkownik utworzony pomyślnie!\n\nHasło jednorazowe (12 znaków, ważne 4h):\n{tempPassword}",
                                "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNewUsername.Clear();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania użytkownika (login może być zajęty): " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Wybierz użytkownika z listy do resetu hasła!", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

            var confirm = MessageBox.Show($"Czy na pewno chcesz zresetować hasło dla użytkownika {username}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    string tempPassword = SecurityService.GenerateTemporaryPassword();
                    var (hash, salt) = SecurityService.HashPassword(tempPassword);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"UPDATE [Users] 
                                       SET PasswordHash = @hash, 
                                           Salt = @salt, 
                                           IsFirstLogin = 1, 
                                           TempPasswordExpiry = DATEADD(hour, 4, GETDATE()) 
                                       WHERE Id = @id";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@hash", hash);
                            cmd.Parameters.AddWithValue("@salt", salt);
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Hasło zostało zresetowane!\n\nNowe hasło jednorazowe (12 znaków, ważne 4h):\n{tempPassword}",
                                    "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas resetowania hasła: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnToggleBlock_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);
            bool currentBlocked = Convert.ToBoolean(dgvUsers.SelectedRows[0].Cells["IsBlocked"].Value);
            bool newBlockedState = !currentBlocked;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE [Users] SET IsBlocked = @blocked WHERE Id = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@blocked", newBlockedState);
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zmiany statusu blokady: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

            var confirm = MessageBox.Show($"Czy na pewno chcesz usunąć użytkownika {username}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM [Users] WHERE Id = @id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd usuwania użytkownika: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtExpiryDays.Text, out int expiryDays) || !int.TryParse(txtSessionTimeout.Text, out int sessionTimeout))
            {
                MessageBox.Show("Ważność hasła i czas bezczynności muszą być liczbami całkowitymi!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE [SystemSettings] 
                                   SET PolicyEnabled = @policy, 
                                       RequireLevenshtein = @levenshtein, 
                                       ExpiryDays = @expiry, 
                                       SessionTimeoutMinutes = @timeout 
                                   WHERE Id = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@policy", chkPolicyEnabled.Checked);
                        cmd.Parameters.AddWithValue("@levenshtein", chkRequireLevenshtein.Checked);
                        cmd.Parameters.AddWithValue("@expiry", expiryDays);
                        cmd.Parameters.AddWithValue("@timeout", sessionTimeout);
                        cmd.ExecuteNonQuery();
                    }
                }

                sessionTimeoutMinutes = sessionTimeout;

                MessageBox.Show("Ustawienia systemowe zostały zapisane pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu ustawień: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
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