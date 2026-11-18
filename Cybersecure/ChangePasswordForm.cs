using Cybersecure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cybersecure
{
    public partial class ChangePasswordForm : Form
    {
        private readonly int _userId;
        private string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Cybersecure;Trusted_Connection=True;";

        public ChangePasswordForm(int userId)
        {
            InitializeComponent();
            _userId = userId;

            txtOldPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblError.Text = "Wypełnij wszystkie pola haseł.";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblError.Text = "Podane nowe hasła nie są identyczne.";
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }

            if (newPassword == oldPassword)
            {
                lblError.Text = "Nowe hasło musi się różnić od starego hasła.";
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
                txtNewPassword.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string userQuery = "SELECT PasswordHash, Salt FROM [Users] WHERE Id = @userId";
                    string settingsQuery = "SELECT PolicyEnabled, RequireLevenshtein FROM [SystemSettings] WHERE Id = 1";

                    string storedHash = "";
                    string storedSalt = "";
                    bool policyEnabled = true;
                    bool requireLevenshtein = true;

                    using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", _userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                storedHash = reader.GetString(0);
                                storedSalt = reader.GetString(1);
                            }
                        }
                    }

                    if (!SecurityService.VerifyPassword(oldPassword, storedHash, storedSalt))
                    {
                        lblError.Text = "Stare hasło jest niepoprawne!";
                        txtOldPassword.Clear();
                        txtOldPassword.Focus();
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand(settingsQuery, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                policyEnabled = reader.GetBoolean(0);
                                requireLevenshtein = reader.GetBoolean(1);
                            }
                        }
                    }

                    var (isValid, errorMessage) = SecurityService.ValidatePasswordPolicy(newPassword, oldPassword, policyEnabled, requireLevenshtein);

                    if (!isValid)
                    {
                        lblError.Text = errorMessage;
                        return;
                    }

                    var (newHash, newSalt) = SecurityService.HashPassword(newPassword);

                    string updateQuery = @"UPDATE [Users] 
                                           SET PasswordHash = @hash, 
                                               Salt = @salt, 
                                               IsFirstLogin = 0, 
                                               PasswordChangedDate = GETDATE(), 
                                               TempPasswordExpiry = DATEADD(day, 5, GETDATE())
                                           WHERE Id = @userId;";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@hash", newHash);
                        cmd.Parameters.AddWithValue("@salt", newSalt);
                        cmd.Parameters.AddWithValue("@userId", _userId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Hasło zostało pomyślnie zmienione.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisu nowego hasła: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}