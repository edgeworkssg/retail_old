namespace WinPowerPOS.OrderForms
{
    partial class frmRenewal
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMembershipNo = new System.Windows.Forms.TextBox();
            this.lblEpxDateNow = new System.Windows.Forms.Label();
            this.dtpNewExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbMembershipGroup = new System.Windows.Forms.ComboBox();
            this.txtMonths = new NullFX.Controls.NumericTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(24, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "EXTENSION";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(207, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "MONTHS";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(162, 169);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 37);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(81, 168);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 37);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(24, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "CURRENT EXPIRY DATE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(24, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "NEW EXPIRY DATE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(24, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "MEMBERSHIP NO";
            // 
            // txtMembershipNo
            // 
            this.txtMembershipNo.Location = new System.Drawing.Point(164, 13);
            this.txtMembershipNo.Name = "txtMembershipNo";
            this.txtMembershipNo.ReadOnly = true;
            this.txtMembershipNo.Size = new System.Drawing.Size(162, 20);
            this.txtMembershipNo.TabIndex = 12;
            // 
            // lblEpxDateNow
            // 
            this.lblEpxDateNow.AutoSize = true;
            this.lblEpxDateNow.BackColor = System.Drawing.Color.Transparent;
            this.lblEpxDateNow.Location = new System.Drawing.Point(173, 108);
            this.lblEpxDateNow.Name = "lblEpxDateNow";
            this.lblEpxDateNow.Size = new System.Drawing.Size(10, 13);
            this.lblEpxDateNow.TabIndex = 13;
            this.lblEpxDateNow.Text = "-";
            // 
            // dtpNewExpiryDate
            // 
            this.dtpNewExpiryDate.CustomFormat = "dd MMMM yyyy";
            this.dtpNewExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNewExpiryDate.Location = new System.Drawing.Point(164, 132);
            this.dtpNewExpiryDate.Name = "dtpNewExpiryDate";
            this.dtpNewExpiryDate.Size = new System.Drawing.Size(162, 20);
            this.dtpNewExpiryDate.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(24, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "MEMBERSHIP GROUP";
            // 
            // cmbMembershipGroup
            // 
            this.cmbMembershipGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMembershipGroup.FormattingEnabled = true;
            this.cmbMembershipGroup.Location = new System.Drawing.Point(164, 42);
            this.cmbMembershipGroup.Name = "cmbMembershipGroup";
            this.cmbMembershipGroup.Size = new System.Drawing.Size(162, 21);
            this.cmbMembershipGroup.TabIndex = 18;
            // 
            // txtMonths
            // 
            this.txtMonths.Location = new System.Drawing.Point(164, 72);
            this.txtMonths.Name = "txtMonths";
            this.txtMonths.Size = new System.Drawing.Size(37, 20);
            this.txtMonths.TabIndex = 15;
            this.txtMonths.Text = "12";
            this.txtMonths.TextChanged += new System.EventHandler(this.txtMonths_TextChanged);
            this.txtMonths.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtMonths_MouseClick);
            // 
            // frmRenewal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinPowerPOS.Properties.Resources.menubackgnd;
            this.ClientSize = new System.Drawing.Size(347, 221);
            this.Controls.Add(this.cmbMembershipGroup);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dtpNewExpiryDate);
            this.Controls.Add(this.txtMonths);
            this.Controls.Add(this.lblEpxDateNow);
            this.Controls.Add(this.txtMembershipNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmRenewal";
            this.Text = "Membership Renewal";
            this.Load += new System.EventHandler(this.frmRenewal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMembershipNo;
        private System.Windows.Forms.Label lblEpxDateNow;
        private NullFX.Controls.NumericTextBox txtMonths;
        private System.Windows.Forms.DateTimePicker dtpNewExpiryDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbMembershipGroup;

    }
}