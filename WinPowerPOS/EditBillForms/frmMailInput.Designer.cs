namespace WinPowerPOS.EditBillForms
{
    partial class frmMailInput
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
            this.btnSend = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtBoxEmail = new System.Windows.Forms.TextBox();
            this.lblMailTo = new System.Windows.Forms.Label();
            this.lblMailSubject = new System.Windows.Forms.Label();
            this.txtBoxSubject = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxMailContent = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.BackColor = System.Drawing.Color.Transparent;
            this.btnSend.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnSend.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSend.Location = new System.Drawing.Point(145, 126);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(70, 26);
            this.btnSend.TabIndex = 83;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::WinPowerPOS.Properties.Resources.blueButton;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(221, 126);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 26);
            this.btnCancel.TabIndex = 84;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtBoxEmail
            // 
            this.txtBoxEmail.Location = new System.Drawing.Point(101, 12);
            this.txtBoxEmail.Name = "txtBoxEmail";
            this.txtBoxEmail.Size = new System.Drawing.Size(190, 20);
            this.txtBoxEmail.TabIndex = 85;
            // 
            // lblMailTo
            // 
            this.lblMailTo.AutoSize = true;
            this.lblMailTo.Location = new System.Drawing.Point(10, 12);
            this.lblMailTo.Name = "lblMailTo";
            this.lblMailTo.Size = new System.Drawing.Size(42, 13);
            this.lblMailTo.TabIndex = 86;
            this.lblMailTo.Text = "Mail To";
            // 
            // lblMailSubject
            // 
            this.lblMailSubject.AutoSize = true;
            this.lblMailSubject.Location = new System.Drawing.Point(10, 43);
            this.lblMailSubject.Name = "lblMailSubject";
            this.lblMailSubject.Size = new System.Drawing.Size(43, 13);
            this.lblMailSubject.TabIndex = 87;
            this.lblMailSubject.Text = "Subject";
            // 
            // txtBoxSubject
            // 
            this.txtBoxSubject.Location = new System.Drawing.Point(101, 40);
            this.txtBoxSubject.Name = "txtBoxSubject";
            this.txtBoxSubject.Size = new System.Drawing.Size(190, 20);
            this.txtBoxSubject.TabIndex = 88;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 89;
            this.label1.Text = "Mail Content";
            // 
            // txtBoxMailContent
            // 
            this.txtBoxMailContent.Location = new System.Drawing.Point(101, 70);
            this.txtBoxMailContent.Name = "txtBoxMailContent";
            this.txtBoxMailContent.Size = new System.Drawing.Size(190, 50);
            this.txtBoxMailContent.TabIndex = 90;
            this.txtBoxMailContent.Text = "";
            // 
            // frmMailInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 164);
            this.Controls.Add(this.txtBoxMailContent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxSubject);
            this.Controls.Add(this.lblMailSubject);
            this.Controls.Add(this.lblMailTo);
            this.Controls.Add(this.txtBoxEmail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMailInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input An Email";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnSend;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtBoxEmail;
        private System.Windows.Forms.Label lblMailTo;
        private System.Windows.Forms.Label lblMailSubject;
        private System.Windows.Forms.TextBox txtBoxSubject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtBoxMailContent;
    }
}