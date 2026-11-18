using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Cybersecure.Services;

namespace Cybersecure
{
    public partial class LoginForm : Form
    {
        public bool IsAuthenticated { get; private set; } = false;
        public string LoggedInRole { get; private set; } = string.Empty;
        public string LoggedInPermissions { get; private set; } = string.Empty; 
        public bool IsFirstLogin { get; private set; } = false;
        public int LoggedInUserId { get; private set; } = 0;

        private string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Cybersecure;Trusted_Connection=True;";

        public LoginForm()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Wypełnij wszystkie pola!";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT u.Id, u.Username, u.PasswordHash, u.Salt, r.RoleName, r.Permissions, 
                                            u.IsBlocked, u.IsFirstLogin, u.TempPasswordExpiry, u.PasswordNeverExpires 
                                     FROM [Users] u 
                                     JOIN [Roles] r ON u.RoleId = r.Id 
                                     WHERE u.[Username] = @username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string dbUsername = reader.GetString(1);
                                string storedHash = reader.GetString(2);
                                string storedSalt = reader.GetString(3);
                                string role = reader.GetString(4);    
                                string permissions = reader.GetString(5); 
                                bool isBlocked = reader.GetBoolean(6);
                                bool isFirstLogin = reader.GetBoolean(7);
                                DateTime? tempExpiry = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8);
                                bool passwordNeverExpired = !reader.IsDBNull(9) && reader.GetBoolean(9);

                                if (isBlocked)
                                {
                                    MessageBox.Show("Twoje konto zostało zablokowane. Skontaktuj się z administratorem.",
                                                    "Konto zablokowane",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);

                                    txtPassword.Clear();
                                    txtPassword.Focus();
                                    return;
                                }

                                if (isFirstLogin && tempExpiry.HasValue && DateTime.Now > tempExpiry.Value)
                                {
                                    MessageBox.Show("Hasło jednorazowe wygasło. Skontaktuj się z administratorem.",
                                                    "Wygasłe hasło", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    txtPassword.Clear();
                                    txtPassword.Focus();
                                    return;
                                }

                                bool isPasswordValid = SecurityService.VerifyPassword(password, storedHash, storedSalt);

                                if (isPasswordValid)
                                {
                                    bool isPasswordExpired = false;

                                    if (!passwordNeverExpired)
                                    {
                                        isPasswordExpired = tempExpiry.HasValue && DateTime.Now >= tempExpiry.Value;
                                    }

                                    IsAuthenticated = true;
                                    LoggedInUserId = id;
                                    LoggedInRole = role;
                                    LoggedInPermissions = permissions; 

                                    IsFirstLogin = (isFirstLogin || isPasswordExpired);

                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    ShowGenericError();
                                }
                            }
                            else
                            {
                                ShowGenericError();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd połączenia z bazą danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowGenericError()
        {
            lblError.Text = "Login lub hasło niepoprawne";
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }
}