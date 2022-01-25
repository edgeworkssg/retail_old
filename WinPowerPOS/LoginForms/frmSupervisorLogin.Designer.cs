namespace WinPowerPOS.LoginForms
{
    partial class frmSupervisorLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSupervisorLogin));
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSupervisorId = new System.Windows.Forms.TextBox();
            this.btnKeyboard = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnKeyboard)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.White;
            this.btnLogin.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Wheat;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSupervisorId
            // 
            resources.ApplyResources(this.txtSupervisorId, "txtSupervisorId");
            this.txtSupervisorId.Name = "txtSupervisorId";
            // 
            // btnKeyboard
            // 
            this.btnKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyboard.ErrorImage = null;
            this.btnKeyboard.Image = global::WinPowerPOS.Properties.Resources.Best_Keyboard_Apps;
            resources.ApplyResources(this.btnKeyboard, "btnKeyboard");
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.TabStop = false;
            this.btnKeyboard.Click += new System.EventHandler(this.btnKeyboard_Click);
            // 
            // frmSupervisorLogin
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ControlBox = false;
            this.Controls.Add(this.btnKeyboard);
            this.Controls.Add(this.txtSupervisorId);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPassword);
            this.Name = "frmSupervisorLogin";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SupervisorLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnKeyboard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSupervisorId;
        private System.Windows.Forms.PictureBox btnKeyboard;
    }
}