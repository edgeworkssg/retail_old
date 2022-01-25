namespace WinPowerPOS.MaintenanceForms
{
    partial class frmFeedback
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
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboRate = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSeverity = new System.Windows.Forms.ComboBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEmailValidator = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Company";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(132, 29);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(258, 22);
            this.txtCompany.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(132, 54);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(258, 22);
            this.txtName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(132, 79);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(258, 22);
            this.txtEmail.TabIndex = 24;
            this.txtEmail.Leave += new System.EventHandler(this.txtEmail_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "Email";
            // 
            // btnSend
            // 
            this.btnSend.BackgroundImage = global::WinPowerPOS.Properties.Resources.greenbutton;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(392, 421);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(168, 43);
            this.btnSend.TabIndex = 34;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(193, 356);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(239, 16);
            this.label7.TabIndex = 33;
            this.label7.Text = "(1 - 5, 1 is very poor and 5 is very good)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 356);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 16);
            this.label6.TabIndex = 32;
            this.label6.Text = "Rate Our Service";
            // 
            // cboRate
            // 
            this.cboRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRate.FormattingEnabled = true;
            this.cboRate.Items.AddRange(new object[] {
            "     ",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cboRate.Location = new System.Drawing.Point(132, 351);
            this.cboRate.Name = "cboRate";
            this.cboRate.Size = new System.Drawing.Size(55, 24);
            this.cboRate.TabIndex = 31;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(132, 156);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(428, 192);
            this.txtDescription.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 16);
            this.label5.TabIndex = 29;
            this.label5.Text = "Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 28;
            this.label4.Text = "Severity";
            // 
            // cboSeverity
            // 
            this.cboSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeverity.FormattingEnabled = true;
            this.cboSeverity.Items.AddRange(new object[] {
            "      ",
            "Low : My day to day work is not impacted at this time",
            "Important : I can do work, but system does not work as expected",
            "Critical : I am not able to do my work"});
            this.cboSeverity.Location = new System.Drawing.Point(132, 129);
            this.cboSeverity.Name = "cboSeverity";
            this.cboSeverity.Size = new System.Drawing.Size(428, 24);
            this.cboSeverity.TabIndex = 27;
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(132, 104);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(258, 22);
            this.txtSubject.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Subject";
            // 
            // lblEmailValidator
            // 
            this.lblEmailValidator.AutoSize = true;
            this.lblEmailValidator.ForeColor = System.Drawing.Color.DarkRed;
            this.lblEmailValidator.Location = new System.Drawing.Point(396, 82);
            this.lblEmailValidator.Name = "lblEmailValidator";
            this.lblEmailValidator.Size = new System.Drawing.Size(84, 16);
            this.lblEmailValidator.TabIndex = 35;
            this.lblEmailValidator.Text = "Invalid Email";
            this.lblEmailValidator.Visible = false;
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::WinPowerPOS.Properties.Resources.redbutton;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(222, 421);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(168, 43);
            this.btnClear.TabIndex = 36;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 483);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblEmailValidator);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboRate);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboSeverity);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCompany);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFeedback";
            this.Text = "Feedback Form";
            this.Load += new System.EventHandler(this.frmFeedback_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboRate;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSeverity;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblEmailValidator;
        private System.Windows.Forms.Button btnClear;
    }
}