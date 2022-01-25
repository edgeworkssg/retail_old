namespace PowerInventory.LoginForms
{
    partial class frmPOSLogin
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnLogin = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.txtUserID);
			this.groupBox2.Controls.Add(this.txtPassword);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.btnCancel);
			this.groupBox2.Controls.Add(this.btnLogin);
			this.groupBox2.Location = new System.Drawing.Point(10, 126);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(321, 139);
			this.groupBox2.TabIndex = 42;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Login";
			// 
			// txtUserID
			// 
			this.txtUserID.Location = new System.Drawing.Point(116, 25);
			this.txtUserID.Name = "txtUserID";
			this.txtUserID.Size = new System.Drawing.Size(191, 20);
			this.txtUserID.TabIndex = 26;
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(116, 56);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(191, 20);
			this.txtPassword.TabIndex = 27;
			this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label9.Location = new System.Drawing.Point(15, 28);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(58, 13);
			this.label9.TabIndex = 28;
			this.label9.Text = "USER ID";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(15, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 29;
			this.label1.Text = "PASSWORD";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::PowerInventory.Properties.Resources.redbutton;
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
			this.btnCancel.ForeColor = System.Drawing.Color.White;
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnCancel.Location = new System.Drawing.Point(221, 82);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(85, 45);
			this.btnCancel.TabIndex = 52;
			this.btnCancel.Text = "CLOSE";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnLogin
			// 
			this.btnLogin.BackColor = System.Drawing.Color.Transparent;
			this.btnLogin.BackgroundImage = global::PowerInventory.Properties.Resources.greenbutton;
			this.btnLogin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
			this.btnLogin.ForeColor = System.Drawing.Color.White;
			this.btnLogin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnLogin.Location = new System.Drawing.Point(127, 82);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(88, 45);
			this.btnLogin.TabIndex = 51;
			this.btnLogin.Text = "LOGIN";
			this.btnLogin.UseVisualStyleBackColor = false;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(0, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(461, 108);
			this.pictureBox1.TabIndex = 43;
			this.pictureBox1.TabStop = false;
			// 
			// frmPOSLogin
			// 
			this.AcceptButton = this.btnLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(339, 275);
			this.ControlBox = false;
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.groupBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmPOSLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.frmPOSLogin_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}