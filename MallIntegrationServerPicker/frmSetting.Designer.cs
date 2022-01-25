namespace MallIntegrationServerPicker
{
    partial class frmSetting
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
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWebServiceURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolderLocation = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtBackupFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCheckInterval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMallCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Chartreuse;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(60, 286);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 43);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Web Service URL";
            // 
            // txtWebServiceURL
            // 
            this.txtWebServiceURL.Location = new System.Drawing.Point(15, 40);
            this.txtWebServiceURL.Name = "txtWebServiceURL";
            this.txtWebServiceURL.Size = new System.Drawing.Size(311, 20);
            this.txtWebServiceURL.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Folder To Check";
            // 
            // txtFolderLocation
            // 
            this.txtFolderLocation.Location = new System.Drawing.Point(15, 92);
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.Size = new System.Drawing.Size(311, 20);
            this.txtFolderLocation.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkRed;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(162, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 43);
            this.button1.TabIndex = 5;
            this.button1.Text = "CANCEL";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(87, 171);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(210, 20);
            this.txtUserName.TabIndex = 7;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(12, 174);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(69, 13);
            this.lblUserName.TabIndex = 6;
            this.lblUserName.Text = "Tenant Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "API Key";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(87, 197);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(210, 20);
            this.txtPassword.TabIndex = 9;
            // 
            // txtBackupFolder
            // 
            this.txtBackupFolder.Location = new System.Drawing.Point(15, 140);
            this.txtBackupFolder.Name = "txtBackupFolder";
            this.txtBackupFolder.Size = new System.Drawing.Size(311, 20);
            this.txtBackupFolder.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Backup Folder";
            // 
            // txtCheckInterval
            // 
            this.txtCheckInterval.Location = new System.Drawing.Point(126, 249);
            this.txtCheckInterval.Name = "txtCheckInterval";
            this.txtCheckInterval.Size = new System.Drawing.Size(171, 20);
            this.txtCheckInterval.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 252);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Check Interval ( sec )";
            // 
            // txtMallCode
            // 
            this.txtMallCode.Location = new System.Drawing.Point(87, 223);
            this.txtMallCode.Name = "txtMallCode";
            this.txtMallCode.Size = new System.Drawing.Size(210, 20);
            this.txtMallCode.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Mall Code";
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 341);
            this.Controls.Add(this.txtMallCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCheckInterval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBackupFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFolderLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWebServiceURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Name = "frmSetting";
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWebServiceURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolderLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtBackupFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCheckInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMallCode;
        private System.Windows.Forms.Label label6;
    }
}