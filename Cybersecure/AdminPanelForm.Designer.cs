namespace Cybersecure
{
    partial class AdminPanelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.groupBoxAddUser = new System.Windows.Forms.GroupBox();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.txtNewUsername = new System.Windows.Forms.TextBox();
            this.lblNewUsername = new System.Windows.Forms.Label();
            this.btnToggleBlock = new System.Windows.Forms.Button();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.txtSessionTimeout = new System.Windows.Forms.TextBox();
            this.lblSessionTimeout = new System.Windows.Forms.Label();
            this.txtExpiryDays = new System.Windows.Forms.TextBox();
            this.lblExpiryDays = new System.Windows.Forms.Label();
            this.chkRequireLevenshtein = new System.Windows.Forms.CheckBox();
            this.chkPolicyEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.groupBoxAddUser.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(12, 12);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(530, 430);
            this.dgvUsers.TabIndex = 0;
            // 
            // groupBoxAddUser
            // 
            this.groupBoxAddUser.Controls.Add(this.btnAddUser);
            this.groupBoxAddUser.Controls.Add(this.cmbRole);
            this.groupBoxAddUser.Controls.Add(this.lblRole);
            this.groupBoxAddUser.Controls.Add(this.txtNewUsername);
            this.groupBoxAddUser.Controls.Add(this.lblNewUsername);
            this.groupBoxAddUser.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.groupBoxAddUser.Location = new System.Drawing.Point(558, 12);
            this.groupBoxAddUser.Name = "groupBoxAddUser";
            this.groupBoxAddUser.Size = new System.Drawing.Size(260, 190);
            this.groupBoxAddUser.TabIndex = 1;
            this.groupBoxAddUser.TabStop = false;
            this.groupBoxAddUser.Text = "Dodaj użytkownika";
            // 
            // btnAddUser
            // 
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.Location = new System.Drawing.Point(15, 140);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(230, 30);
            this.btnAddUser.TabIndex = 4;
            this.btnAddUser.Text = "Utwórz konto i generuj OTP";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "Administrator",
            "Kierownik",
            "Redaktor",
            "Pracownik",
            "Czytelnik"});
            this.cmbRole.Location = new System.Drawing.Point(15, 95);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(230, 25);
            this.cmbRole.TabIndex = 3;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(12, 75);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(35, 17);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Rola:";
            // 
            // txtNewUsername
            // 
            this.txtNewUsername.Location = new System.Drawing.Point(15, 42);
            this.txtNewUsername.Name = "txtNewUsername";
            this.txtNewUsername.Size = new System.Drawing.Size(230, 25);
            this.txtNewUsername.TabIndex = 1;
            // 
            // lblNewUsername
            // 
            this.lblNewUsername.AutoSize = true;
            this.lblNewUsername.Location = new System.Drawing.Point(12, 22);
            this.lblNewUsername.Name = "lblNewUsername";
            this.lblNewUsername.Size = new System.Drawing.Size(127, 17);
            this.lblNewUsername.TabIndex = 0;
            this.lblNewUsername.Text = "Nazwa użytkownika:";
            // 
            // btnToggleBlock
            // 
            this.btnToggleBlock.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnToggleBlock.Location = new System.Drawing.Point(12, 455);
            this.btnToggleBlock.Name = "btnToggleBlock";
            this.btnToggleBlock.Size = new System.Drawing.Size(120, 30);
            this.btnToggleBlock.TabIndex = 2;
            this.btnToggleBlock.Text = "Zablokuj / Odblokuj";
            this.btnToggleBlock.UseVisualStyleBackColor = true;
            this.btnToggleBlock.Click += new System.EventHandler(this.btnToggleBlock_Click);
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnResetPassword.Location = new System.Drawing.Point(138, 455);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(95, 30);
            this.btnResetPassword.TabIndex = 5;
            this.btnResetPassword.Text = "Resetuj hasło";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDeleteUser.ForeColor = System.Drawing.Color.Red;
            this.btnDeleteUser.Location = new System.Drawing.Point(239, 455);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(95, 30);
            this.btnDeleteUser.TabIndex = 3;
            this.btnDeleteUser.Text = "Usuń";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Location = new System.Drawing.Point(442, 455);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Wyloguj";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.btnSaveSettings);
            this.groupBoxSettings.Controls.Add(this.txtSessionTimeout);
            this.groupBoxSettings.Controls.Add(this.lblSessionTimeout);
            this.groupBoxSettings.Controls.Add(this.txtExpiryDays);
            this.groupBoxSettings.Controls.Add(this.lblExpiryDays);
            this.groupBoxSettings.Controls.Add(this.chkRequireLevenshtein);
            this.groupBoxSettings.Controls.Add(this.chkPolicyEnabled);
            this.groupBoxSettings.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.groupBoxSettings.Location = new System.Drawing.Point(558, 215);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(260, 270);
            this.groupBoxSettings.TabIndex = 4;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Ustawienia systemowe";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSaveSettings.Location = new System.Drawing.Point(15, 220);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(230, 30);
            this.btnSaveSettings.TabIndex = 6;
            this.btnSaveSettings.Text = "Zapisz ustawienia";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // txtSessionTimeout
            // 
            this.txtSessionTimeout.Location = new System.Drawing.Point(15, 175);
            this.txtSessionTimeout.Name = "txtSessionTimeout";
            this.txtSessionTimeout.Size = new System.Drawing.Size(230, 25);
            this.txtSessionTimeout.TabIndex = 5;
            // 
            // lblSessionTimeout
            // 
            this.lblSessionTimeout.AutoSize = true;
            this.lblSessionTimeout.Location = new System.Drawing.Point(12, 155);
            this.lblSessionTimeout.Name = "lblSessionTimeout";
            this.lblSessionTimeout.Size = new System.Drawing.Size(206, 17);
            this.lblSessionTimeout.TabIndex = 4;
            this.lblSessionTimeout.Text = "Czas bezczynności sesji (w min.):";
            // 
            // txtExpiryDays
            // 
            this.txtExpiryDays.Location = new System.Drawing.Point(15, 120);
            this.txtExpiryDays.Name = "txtExpiryDays";
            this.txtExpiryDays.Size = new System.Drawing.Size(230, 25);
            this.txtExpiryDays.TabIndex = 3;
            // 
            // lblExpiryDays
            // 
            this.lblExpiryDays.AutoSize = true;
            this.lblExpiryDays.Location = new System.Drawing.Point(12, 100);
            this.lblExpiryDays.Name = "lblExpiryDays";
            this.lblExpiryDays.Size = new System.Drawing.Size(158, 17);
            this.lblExpiryDays.TabIndex = 2;
            this.lblExpiryDays.Text = "Ważność hasła (w dniach):";
            // 
            // chkRequireLevenshtein
            // 
            this.chkRequireLevenshtein.AutoSize = true;
            this.chkRequireLevenshtein.Location = new System.Drawing.Point(15, 60);
            this.chkRequireLevenshtein.Name = "chkRequireLevenshtein";
            this.chkRequireLevenshtein.Size = new System.Drawing.Size(211, 21);
            this.chkRequireLevenshtein.TabIndex = 1;
            this.chkRequireLevenshtein.Text = "Reguła Levenshteina (min. 4 różn.)";
            this.chkRequireLevenshtein.UseVisualStyleBackColor = true;
            // 
            // chkPolicyEnabled
            // 
            this.chkPolicyEnabled.AutoSize = true;
            this.chkPolicyEnabled.Location = new System.Drawing.Point(15, 30);
            this.chkPolicyEnabled.Name = "chkPolicyEnabled";
            this.chkPolicyEnabled.Size = new System.Drawing.Size(147, 21);
            this.chkPolicyEnabled.TabIndex = 0;
            this.chkPolicyEnabled.Text = "Włącz politykę haseł";
            this.chkPolicyEnabled.UseVisualStyleBackColor = true;
            // 
            // AdminPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 501);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.btnToggleBlock);
            this.Controls.Add(this.groupBoxAddUser);
            this.Controls.Add(this.dgvUsers);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminPanelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Panel Administratora - Cybersecure";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.groupBoxAddUser.ResumeLayout(false);
            this.groupBoxAddUser.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.GroupBox groupBoxAddUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.TextBox txtNewUsername;
        private System.Windows.Forms.Label lblNewUsername;
        private System.Windows.Forms.Button btnToggleBlock;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.TextBox txtSessionTimeout;
        private System.Windows.Forms.Label lblSessionTimeout;
        private System.Windows.Forms.TextBox txtExpiryDays;
        private System.Windows.Forms.Label lblExpiryDays;
        private System.Windows.Forms.CheckBox chkRequireLevenshtein;
        private System.Windows.Forms.CheckBox chkPolicyEnabled;
    }
}